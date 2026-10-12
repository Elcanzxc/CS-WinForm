import { NextRequest, NextResponse } from "next/server";
import { getDb } from "@/lib/db";
import { evaluateTriageCDSS, VitalsInput } from "@/lib/triageEngine";

export async function GET(request: NextRequest) {
  try {
    const db = getDb();
    const { searchParams } = new URL(request.url);
    const search = searchParams.get("search") || "";
    const level = searchParams.get("level");
    const status = searchParams.get("status");

    let query = `
      SELECT p.*, 
             t.sys, t.dia, t.pulse, t.spo2, t.temp, t.glucose, t.vas_pain, 
             t.symptoms_json, t.triage_wait_time, t.suspected_diagnosis, 
             t.icd10_code, t.emergency_exams_json, t.clinical_notes, t.evaluated_at
      FROM patients p
      LEFT JOIN triage_records t ON p.id = t.patient_id
      WHERE 1=1
    `;
    const params: any[] = [];

    if (search) {
      query += ` AND (p.full_name LIKE ? OR p.fin LIKE ? OR p.ticket_number LIKE ? OR p.phone LIKE ?)`;
      const s = `%${search}%`;
      params.push(s, s, s, s);
    }

    if (level && level !== "ALL") {
      query += ` AND p.triage_level = ?`;
      params.push(parseInt(level, 10));
    }

    if (status && status !== "ALL") {
      query += ` AND p.status = ?`;
      params.push(status);
    }

    query += ` ORDER BY p.id DESC LIMIT 100`;

    const stmt = db.prepare(query);
    const rows = stmt.all(...params);

    return NextResponse.json({ success: true, data: rows });
  } catch (error: any) {
    return NextResponse.json({ success: false, error: error.message }, { status: 500 });
  }
}

export async function POST(request: NextRequest) {
  try {
    const db = getDb();
    const body = await request.json();

    const {
      fin,
      fullName,
      birthDate,
      gender,
      phone,
      insuranceType,
      vitals,
      symptoms = [],
      clinicalNotes = "",
    }: {
      fin: string;
      fullName: string;
      birthDate: string;
      gender: string;
      phone: string;
      insuranceType: string;
      vitals: VitalsInput;
      symptoms: string[];
      clinicalNotes?: string;
    } = body;

    if (!fin || !fullName || !birthDate || !phone) {
      return NextResponse.json(
        { success: false, error: "Zəhmət olmasa bütün tələb olunan sahələri doldurun (FİN, Ad/Soyad, Doğum tarixi, Telefon)" },
        { status: 400 }
      );
    }

    // Run CDSS Expert Engine
    const cdss = evaluateTriageCDSS(
      vitals || { sys: 120, dia: 80, pulse: 75, spo2: 98, temp: 36.6, glucose: 5.0, vasPain: 0 },
      symptoms
    );

    // Generate Queue Ticket Number
    // EMG-XX for Level 1 & 2; T-XXX for Level 3, 4, 5
    const isEmergency = cdss.triageLevel <= 2;
    const prefix = isEmergency ? "EMG" : "T";
    const countRow = db
      .prepare("SELECT COUNT(*) as count FROM patients WHERE ticket_number LIKE ?")
      .get(`${prefix}-%`) as { count: number };
    const nextNum = (countRow.count + 1).toString().padStart(isEmergency ? 2 : 3, "0");
    const ticketNumber = `${prefix}-${nextNum}`;

    const initialStatus =
      cdss.triageLevel === 1
        ? "Təxirəsalınmaz Reanimasiya"
        : cdss.triageLevel === 2
        ? "Təcili çağırış"
        : "Gözləyir";

    // Insert Patient
    const insertPatient = db.prepare(`
      INSERT INTO patients (
        ticket_number, fin, full_name, birth_date, gender, phone, 
        insurance_type, status, triage_level, triage_color, 
        assigned_department, assigned_cabinet, assigned_doctor
      ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
    `);

    const result = insertPatient.run(
      ticketNumber,
      fin.toUpperCase().trim(),
      fullName.trim(),
      birthDate,
      gender || "Kişi",
      phone.trim(),
      insuranceType || "İcbari Tibbi Sığorta",
      initialStatus,
      cdss.triageLevel,
      cdss.triageColor,
      cdss.assignedDepartment,
      cdss.assignedCabinet,
      cdss.assignedSpecialist
    );

    const patientId = result.lastInsertRowid;

    // Insert Triage Record
    const insertTriage = db.prepare(`
      INSERT INTO triage_records (
        patient_id, sys, dia, pulse, spo2, temp, glucose, vas_pain,
        symptoms_json, triage_level, triage_color, triage_wait_time,
        suspected_diagnosis, icd10_code, emergency_exams_json, clinical_notes
      ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
    `);

    insertTriage.run(
      patientId,
      vitals?.sys ?? 120,
      vitals?.dia ?? 80,
      vitals?.pulse ?? 75,
      vitals?.spo2 ?? 98,
      vitals?.temp ?? 36.6,
      vitals?.glucose ?? 5.0,
      vitals?.vasPain ?? 0,
      JSON.stringify(symptoms),
      cdss.triageLevel,
      cdss.triageColor,
      cdss.triageWaitTime,
      cdss.suspectedDiagnosis,
      cdss.icd10Code,
      JSON.stringify(cdss.emergencyExams),
      clinicalNotes || cdss.clinicalSummary
    );

    // If critical alert triggered, create Emergency Alert record
    let alertId: number | bigint | null = null;
    if (cdss.emergencyTrigger && cdss.alertType !== "NONE") {
      const insertAlert = db.prepare(`
        INSERT INTO emergency_alerts (code_type, patient_id, ticket_number, patient_name, location, reason, status)
        VALUES (?, ?, ?, ?, ?, ?, 'ACTIVE')
      `);
      const alertRes = insertAlert.run(
        cdss.alertType,
        patientId,
        ticketNumber,
        fullName,
        `${cdss.assignedDepartment} / ${cdss.assignedCabinet}`,
        cdss.emergencyReason || cdss.suspectedDiagnosis
      );
      alertId = alertRes.lastInsertRowid;

      // Update cabinet status to "Təcili çağırış"
      db.prepare(`UPDATE cabinets SET status = 'Təcili çağırış', current_patient_ticket = ? WHERE cabinet_no = ?`).run(
        ticketNumber,
        cdss.assignedCabinet
      );
    }

    // Log in Audit Trail
    db.prepare(`INSERT INTO audit_logs (action_type, description, user_role) VALUES (?, ?, ?)`).run(
      "PATIENT_REGISTERED",
      `Yeni pasiyent qeydiyyatdan keçdi: ${ticketNumber} (${fullName}), MTS Səviyyə: ${cdss.triageLevel} (${cdss.triageColor})`,
      "Triaq Reçepşn"
    );

    return NextResponse.json({
      success: true,
      data: {
        patientId,
        ticketNumber,
        cdss,
        alertId,
      },
    });
  } catch (error: any) {
    return NextResponse.json({ success: false, error: error.message }, { status: 500 });
  }
}
