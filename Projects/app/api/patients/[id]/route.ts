import { NextRequest, NextResponse } from "next/server";
import { getDb } from "@/lib/db";

export async function GET(
  request: NextRequest,
  { params }: { params: Promise<{ id: string }> }
) {
  try {
    const db = getDb();
    const { id } = await params;

    const patient = db
      .prepare(`
        SELECT p.*, 
               t.sys, t.dia, t.pulse, t.spo2, t.temp, t.glucose, t.vas_pain, 
               t.symptoms_json, t.triage_wait_time, t.suspected_diagnosis, 
               t.icd10_code, t.emergency_exams_json, t.clinical_notes, t.evaluated_at
        FROM patients p
        LEFT JOIN triage_records t ON p.id = t.patient_id
        WHERE p.id = ?
      `)
      .get(id);

    if (!patient) {
      return NextResponse.json({ success: false, error: "Pasiyent tapılmadı" }, { status: 404 });
    }

    return NextResponse.json({ success: true, data: patient });
  } catch (error: any) {
    return NextResponse.json({ success: false, error: error.message }, { status: 500 });
  }
}

export async function PATCH(
  request: NextRequest,
  { params }: { params: Promise<{ id: string }> }
) {
  try {
    const db = getDb();
    const { id } = await params;
    const body = await request.json();

    const { status, assignedCabinet, assignedDoctor } = body;

    const updates: string[] = [];
    const values: any[] = [];

    if (status) {
      updates.push("status = ?");
      values.push(status);
    }
    if (assignedCabinet) {
      updates.push("assigned_cabinet = ?");
      values.push(assignedCabinet);
    }
    if (assignedDoctor) {
      updates.push("assigned_doctor = ?");
      values.push(assignedDoctor);
    }

    if (updates.length === 0) {
      return NextResponse.json({ success: false, error: "Yeniləmək üçün məlumat göndərilməyib" }, { status: 400 });
    }

    values.push(id);
    const sql = `UPDATE patients SET ${updates.join(", ")} WHERE id = ?`;
    db.prepare(sql).run(...values);

    db.prepare(`INSERT INTO audit_logs (action_type, description, user_role) VALUES (?, ?, ?)`).run(
      "PATIENT_UPDATED",
      `Pasiyent ID ${id} statusu yeniləndi: ${status || "Məlumatlar dəyişdirildi"}`,
      "Operator"
    );

    return NextResponse.json({ success: true });
  } catch (error: any) {
    return NextResponse.json({ success: false, error: error.message }, { status: 500 });
  }
}
