import { NextRequest, NextResponse } from "next/server";
import { getDb } from "@/lib/db";

export async function POST(request: NextRequest) {
  try {
    const db = getDb();
    const body = await request.json();
    const { cabinetNo, department } = body;

    if (!cabinetNo) {
      return NextResponse.json(
        { success: false, error: "Kabinet nömrəsi tələb olunur" },
        { status: 400 }
      );
    }

    // 1. Find next waiting patient in queue
    // Prioritize MTS Level 1 first, then Level 2, 3, 4, 5, then earliest registered_at
    let query = `
      SELECT * FROM patients
      WHERE status IN ('Gözləyir', 'Təcili çağırış')
    `;
    const params: any[] = [];

    if (department) {
      query += ` AND (assigned_department = ? OR assigned_cabinet = ?)`;
      params.push(department, cabinetNo);
    }

    query += ` ORDER BY triage_level ASC, id ASC LIMIT 1`;

    let nextPatient = db.prepare(query).get(...params) as any;

    // If no patient specifically for this department, take next general waiting patient
    if (!nextPatient) {
      nextPatient = db
        .prepare(`
          SELECT * FROM patients 
          WHERE status IN ('Gözləyir', 'Təcili çağırış')
          ORDER BY triage_level ASC, id ASC LIMIT 1
        `)
        .get() as any;
    }

    if (!nextPatient) {
      return NextResponse.json({
        success: false,
        message: "Növbədə gözləyən pasiyent yoxdur. Bütün xəstələr qəbul edilib.",
      });
    }

    // 2. Update patient status to 'Həkim qəbulunda' and assign to this cabinet
    db.prepare(`
      UPDATE patients 
      SET status = 'Həkim qəbulunda', assigned_cabinet = ?
      WHERE id = ?
    `).run(cabinetNo, nextPatient.id);

    // 3. Update cabinet status
    db.prepare(`
      UPDATE cabinets 
      SET status = 'Xəstə qəbulunda', current_patient_ticket = ?, updated_at = CURRENT_TIMESTAMP
      WHERE cabinet_no = ?
    `).run(nextPatient.ticket_number, cabinetNo);

    // 4. Log in audit trail
    db.prepare(`
      INSERT INTO audit_logs (action_type, description, user_role)
      VALUES (?, ?, ?)
    `).run(
      "QUEUE_CALL_NEXT",
      `Kabinet ${cabinetNo} növbəti pasiyenti çağırdı: ${nextPatient.ticket_number} (${nextPatient.full_name}, MTS Səviyyə ${nextPatient.triage_level})`,
      "Növbətçi Həkim"
    );

    return NextResponse.json({
      success: true,
      patient: nextPatient,
    });
  } catch (error: any) {
    return NextResponse.json({ success: false, error: error.message }, { status: 500 });
  }
}
