import { NextRequest, NextResponse } from "next/server";
import { getDb } from "@/lib/db";

export async function GET(request: NextRequest) {
  try {
    const db = getDb();
    const { searchParams } = new URL(request.url);
    const alertId = searchParams.get("alertId");

    const activeAlerts = db
      .prepare(`
        SELECT * FROM emergency_alerts 
        WHERE status = 'ACTIVE' 
        ORDER BY id DESC
      `)
      .all();

    const recentAlerts = db
      .prepare(`
        SELECT * FROM emergency_alerts 
        ORDER BY id DESC LIMIT 25
      `)
      .all();

    let dispatchesQuery = `SELECT * FROM emergency_dispatches`;
    const params: any[] = [];
    if (alertId) {
      dispatchesQuery += ` WHERE alert_id = ?`;
      params.push(alertId);
    }
    dispatchesQuery += ` ORDER BY id DESC LIMIT 30`;

    const recentDispatches = db.prepare(dispatchesQuery).all(...params);

    // List of on-duty doctors for quick dispatch
    const doctors = db
      .prepare(`
        SELECT cabinet_no, department, doctor_name, doctor_specialty, phone_ext, status 
        FROM cabinets 
        ORDER BY department ASC
      `)
      .all();

    return NextResponse.json({
      success: true,
      activeAlerts,
      recentAlerts,
      recentDispatches,
      doctors,
    });
  } catch (error: any) {
    return NextResponse.json({ success: false, error: error.message }, { status: 500 });
  }
}

export async function POST(request: NextRequest) {
  try {
    const db = getDb();
    const body = await request.json();
    const { codeType, location, reason, patientName, ticketNumber } = body;

    if (!codeType || !location) {
      return NextResponse.json({ success: false, error: "Həyəcan kodu və məkan qeyd olunmalıdır" }, { status: 400 });
    }

    const insert = db.prepare(`
      INSERT INTO emergency_alerts (code_type, location, reason, patient_name, ticket_number, status)
      VALUES (?, ?, ?, ?, ?, 'ACTIVE')
    `);

    const result = insert.run(
      codeType,
      location,
      reason || "Təcili çağırış aktivləşdirildi",
      patientName || "Bilinməyən pasiyent",
      ticketNumber || "PANIC-CALL"
    );

    db.prepare(`INSERT INTO audit_logs (action_type, description, user_role) VALUES (?, ?, ?)`).run(
      "EMERGENCY_ALARM_TRIGGERED",
      `Təcili həyəcan siqnalı aktivləşdirildi: ${codeType} - Məkan: ${location}. Səbəb: ${reason}`,
      "Təcili Dispetçer"
    );

    return NextResponse.json({ success: true, alertId: result.lastInsertRowid });
  } catch (error: any) {
    return NextResponse.json({ success: false, error: error.message }, { status: 500 });
  }
}

export async function PATCH(request: NextRequest) {
  try {
    const db = getDb();
    const body = await request.json();
    const {
      alertId,
      action,
      acknowledgedBy,
      doctorName,
      doctorSpecialty,
      cabinetNo,
      ambulanceTeam,
    } = body;

    if (!alertId || !action) {
      return NextResponse.json({ success: false, error: "Alert ID və action tələb olunur" }, { status: 400 });
    }

    if (action === "ACKNOWLEDGE") {
      db.prepare(`
        UPDATE emergency_alerts 
        SET status = 'ACKNOWLEDGED', acknowledged_at = CURRENT_TIMESTAMP, acknowledged_by = ?
        WHERE id = ?
      `).run(acknowledgedBy || "Növbətçi Triaq Həkimi", alertId);

      db.prepare(`INSERT INTO audit_logs (action_type, description, user_role) VALUES (?, ?, ?)`).run(
        "EMERGENCY_ALARM_ACKNOWLEDGED",
        `Həyəcan siqnalı ID #${alertId} təsdiq edildi və səs dayandırıldı. Təsdiq edən: ${acknowledgedBy || "Növbətçi Həkim"}`,
        "Növbətçi Həkim"
      );

      return NextResponse.json({
        success: true,
        message: "Həyəcan siqnalı təsdiqləndi və arxivə göndərildi",
      });
    }

    if (action === "CALL_AMBULANCE") {
      const team = ambulanceTeam || "Reanimasiya və Şok Zalı Briqadası (#1)";
      const cab = cabinetNo || "REA-01";

      // 1. Mark alert
      db.prepare(`UPDATE emergency_alerts SET ambulance_called = 1 WHERE id = ?`).run(alertId);

      // 2. Put targeted cabinet on Emergency Call status
      db.prepare(`UPDATE cabinets SET status = 'Təcili çağırış' WHERE cabinet_no = ?`).run(cab);

      // 3. Log real dispatch record
      const dispatchInsert = db.prepare(`
        INSERT INTO emergency_dispatches (alert_id, dispatch_type, target_team, channel, status, message)
        VALUES (?, 'AMBULANCE_CALL', ?, 'Radio Kanal 1 (Təcili Şəbəkə)', 'Çatdırıldı (Aktiv)', ?)
      `);
      const dispResult = dispatchInsert.run(
        alertId,
        team,
        `Kritik Reanimasiya Briqadası hadisə yerinə çağırıldı (Kabinet: ${cab})`
      );

      // 4. Log in Audit Trail
      db.prepare(`INSERT INTO audit_logs (action_type, description, user_role) VALUES (?, ?, ?)`).run(
        "AMBULANCE_DISPATCHED",
        `${team} çağırıldı. Radio Kanal 1 aktivdir. Alert #${alertId}. Kabinet ${cab} Təcili Çağırış rejiminə keçirildi.`,
        "Triaq Dispetçeri"
      );

      return NextResponse.json({
        success: true,
        dispatchId: dispResult.lastInsertRowid,
        team,
        message: `${team} çağırıldı! Kabinet ${cab} təcili rejimdədir.`,
        time: new Date().toLocaleTimeString("az-AZ"),
      });
    }

    if (action === "NOTIFY_DOCTOR") {
      const selectedDoc = doctorName || "Dr. Kamran Əliyev";
      const specialty = doctorSpecialty || "Anestezioloq-Reanimatoloq";
      const cab = cabinetNo || "REA-01";

      // 1. Mark alert
      db.prepare(`UPDATE emergency_alerts SET doctor_notified = 1 WHERE id = ?`).run(alertId);

      // 2. Update targeted cabinet status to 'Təcili çağırış'
      if (cab) {
        db.prepare(`UPDATE cabinets SET status = 'Təcili çağırış' WHERE cabinet_no = ?`).run(cab);
      }

      // 3. Log real pager dispatch record
      const dispatchInsert = db.prepare(`
        INSERT INTO emergency_dispatches (alert_id, dispatch_type, target_team, channel, status, message)
        VALUES (?, 'DOCTOR_PAGER', ?, ?, 'Çatdırıldı (SMS Ok)', ?)
      `);
      const dispResult = dispatchInsert.run(
        alertId,
        `${selectedDoc} (${specialty})`,
        `GSM Pager (${cab})`,
        `Klinik Təcili Konsultasiya Çağırışı göndərildi: Alert #${alertId}`
      );

      // 4. Log in Audit Trail
      db.prepare(`INSERT INTO audit_logs (action_type, description, user_role) VALUES (?, ?, ?)`).run(
        "DOCTOR_NOTIFIED",
        `Növbətçi həkim ${selectedDoc} (${specialty}, Kabinet: ${cab}) üçün təcili GSM/Pager bildirişi göndərildi. Alert #${alertId}.`,
        "Triaq Dispetçeri"
      );

      return NextResponse.json({
        success: true,
        dispatchId: dispResult.lastInsertRowid,
        doctor: selectedDoc,
        message: `${selectedDoc} (${specialty}) həkiminə təcili bildiriş çatdırıldı!`,
        time: new Date().toLocaleTimeString("az-AZ"),
      });
    }

    if (action === "RESET_DISPATCHES") {
      db.prepare(`DELETE FROM emergency_dispatches WHERE alert_id = ?`).run(alertId);
      db.prepare(`UPDATE emergency_alerts SET ambulance_called = 0, doctor_notified = 0 WHERE id = ?`).run(alertId);
      return NextResponse.json({ success: true, message: "Çağırış bildirişləri sıfırlandı" });
    }

    if (action === "RESOLVE") {
      db.prepare(`
        UPDATE emergency_alerts 
        SET status = 'RESOLVED'
        WHERE id = ?
      `).run(alertId);

      db.prepare(`INSERT INTO audit_logs (action_type, description, user_role) VALUES (?, ?, ?)`).run(
        "EMERGENCY_ALARM_RESOLVED",
        `Həyəcan siqnalı ID #${alertId} həll edildi və bağlandı.`,
        "Baş Həkim"
      );

      return NextResponse.json({ success: true, message: "Həyəcan həll edildi" });
    }

    return NextResponse.json({ success: false, error: "Bilinməyən əməliyyat" }, { status: 400 });
  } catch (error: any) {
    return NextResponse.json({ success: false, error: error.message }, { status: 500 });
  }
}
