import { NextRequest, NextResponse } from "next/server";
import { getDb } from "@/lib/db";

export async function GET() {
  try {
    const db = getDb();
    const cabinets = db
      .prepare(`
        SELECT c.*, 
               p.full_name as current_patient_name,
               p.triage_level as current_patient_level,
               p.triage_color as current_patient_color
        FROM cabinets c
        LEFT JOIN patients p ON c.current_patient_ticket = p.ticket_number
        ORDER BY c.cabinet_no ASC
      `)
      .all();

    const departments = db.prepare("SELECT * FROM departments ORDER BY id ASC").all();

    return NextResponse.json({ success: true, cabinets, departments });
  } catch (error: any) {
    return NextResponse.json({ success: false, error: error.message }, { status: 500 });
  }
}

export async function PATCH(request: NextRequest) {
  try {
    const db = getDb();
    const body = await request.json();
    const { cabinetNo, status, currentPatientTicket } = body;

    if (!cabinetNo || !status) {
      return NextResponse.json({ success: false, error: "Kabinet nömrəsi və status tələb olunur" }, { status: 400 });
    }

    db.prepare(`
      UPDATE cabinets 
      SET status = ?, current_patient_ticket = ?, updated_at = CURRENT_TIMESTAMP 
      WHERE cabinet_no = ?
    `).run(status, currentPatientTicket || null, cabinetNo);

    // If patient ticket was assigned and status is "Xəstə qəbulunda", update patient status as well
    if (currentPatientTicket && status === "Xəstə qəbulunda") {
      db.prepare(`UPDATE patients SET status = 'Həkim qəbulunda' WHERE ticket_number = ?`).run(currentPatientTicket);
    } else if (status === "Boşdur" && currentPatientTicket) {
      db.prepare(`UPDATE patients SET status = 'Tamamlandı' WHERE ticket_number = ?`).run(currentPatientTicket);
    }

    db.prepare(`INSERT INTO audit_logs (action_type, description, user_role) VALUES (?, ?, ?)`).run(
      "CABINET_STATUS_CHANGE",
      `Kabinet ${cabinetNo} statusu dəyişdirildi: ${status} (Xəstə: ${currentPatientTicket || "Yoxdur"})`,
      "Dispetçer"
    );

    return NextResponse.json({ success: true });
  } catch (error: any) {
    return NextResponse.json({ success: false, error: error.message }, { status: 500 });
  }
}
