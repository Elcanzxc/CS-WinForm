import { NextResponse } from "next/server";
import { getDb } from "@/lib/db";

export async function GET() {
  try {
    const db = getDb();

    // Counts by triage level
    const triageCounts = db
      .prepare(`
        SELECT triage_level, COUNT(*) as count 
        FROM patients 
        GROUP BY triage_level
      `)
      .all() as { triage_level: number; count: number }[];

    // Level map
    const levelMap: Record<number, number> = { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 };
    triageCounts.forEach((r) => {
      levelMap[r.triage_level] = r.count;
    });

    // Total patients
    const totalRow = db.prepare("SELECT COUNT(*) as cnt FROM patients").get() as { cnt: number };
    const totalPatients = totalRow.cnt;

    // Status breakdown
    const statusCounts = db
      .prepare("SELECT status, COUNT(*) as count FROM patients GROUP BY status")
      .all() as { status: string; count: number }[];

    // Cabinet statuses
    const cabinetCounts = db
      .prepare("SELECT status, COUNT(*) as count FROM cabinets GROUP BY status")
      .all() as { status: string; count: number }[];

    // Active emergency count
    const activeEmergencies = db
      .prepare("SELECT COUNT(*) as cnt FROM emergency_alerts WHERE status = 'ACTIVE'")
      .get() as { cnt: number };

    // Recent audit logs
    const auditLogs = db
      .prepare("SELECT * FROM audit_logs ORDER BY id DESC LIMIT 15")
      .all();

    // Department loads
    const deptLoads = db
      .prepare(`
        SELECT assigned_department as department, COUNT(*) as patient_count 
        FROM patients 
        WHERE status != 'Tamamlandı'
        GROUP BY assigned_department
      `)
      .all();

    return NextResponse.json({
      success: true,
      stats: {
        totalPatients,
        activeEmergencies: activeEmergencies.cnt,
        triageDistribution: {
          level1Red: levelMap[1],
          level2Orange: levelMap[2],
          level3Yellow: levelMap[3],
          level4Green: levelMap[4],
          level5Blue: levelMap[5],
        },
        statusCounts,
        cabinetCounts,
        deptLoads,
      },
      auditLogs,
    });
  } catch (error: any) {
    return NextResponse.json({ success: false, error: error.message }, { status: 500 });
  }
}
