import Database from "better-sqlite3";
import path from "node:path";
import fs from "node:fs";

const DB_PATH = path.join(process.cwd(), "clinic.db");

let dbInstance: Database.Database | null = null;

export function getDb(): Database.Database {
  if (dbInstance) {
    return dbInstance;
  }

  const isNew = !fs.existsSync(DB_PATH);
  const db = new Database(DB_PATH);
  db.pragma("journal_mode = WAL");
  db.pragma("foreign_keys = ON");

  initSchema(db);

  if (isNew) {
    seedInitialData(db);
  } else {
    // If table count is 0, seed
    const tableCount = db
      .prepare("SELECT count(*) as cnt FROM sqlite_master WHERE type='table' AND name='patients'")
      .get() as { cnt: number };
    if (tableCount.cnt === 0) {
      seedInitialData(db);
    }
  }

  dbInstance = db;
  return db;
}

function initSchema(db: Database.Database) {
  db.exec(`
    CREATE TABLE IF NOT EXISTS departments (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      code TEXT UNIQUE NOT NULL,
      name TEXT NOT NULL,
      head_doctor TEXT NOT NULL,
      floor TEXT NOT NULL,
      active_cases INTEGER DEFAULT 0
    );

    CREATE TABLE IF NOT EXISTS cabinets (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      cabinet_no TEXT UNIQUE NOT NULL,
      department TEXT NOT NULL,
      doctor_name TEXT NOT NULL,
      doctor_specialty TEXT NOT NULL,
      status TEXT NOT NULL DEFAULT 'Boşdur', -- 'Boşdur', 'Xəstə qəbulunda', 'Təcili çağırış', 'Fasilə'
      current_patient_ticket TEXT,
      floor TEXT NOT NULL,
      phone_ext TEXT NOT NULL,
      updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
    );

    CREATE TABLE IF NOT EXISTS patients (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      ticket_number TEXT UNIQUE NOT NULL,
      fin TEXT NOT NULL,
      full_name TEXT NOT NULL,
      birth_date TEXT NOT NULL,
      gender TEXT NOT NULL,
      phone TEXT NOT NULL,
      insurance_type TEXT NOT NULL, -- 'İcbari Tibbi Sığorta', 'Özəl Sığorta', 'Nağd / Öz Hesabına'
      registered_at DATETIME DEFAULT CURRENT_TIMESTAMP,
      status TEXT NOT NULL DEFAULT 'Gözləyir', -- 'Gözləyir', 'Həkim qəbulunda', 'Müayinədə', 'Tamamlandı', 'Təxirəsalınmaz Reanimasiya'
      triage_level INTEGER NOT NULL DEFAULT 4,
      triage_color TEXT NOT NULL DEFAULT 'Yaşıl',
      assigned_department TEXT NOT NULL,
      assigned_cabinet TEXT NOT NULL,
      assigned_doctor TEXT NOT NULL
    );

    CREATE TABLE IF NOT EXISTS triage_records (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      patient_id INTEGER NOT NULL REFERENCES patients(id) ON DELETE CASCADE,
      sys INTEGER NOT NULL,
      dia INTEGER NOT NULL,
      pulse INTEGER NOT NULL,
      spo2 INTEGER NOT NULL,
      temp REAL NOT NULL,
      glucose REAL NOT NULL,
      vas_pain INTEGER NOT NULL,
      symptoms_json TEXT NOT NULL,
      triage_level INTEGER NOT NULL,
      triage_color TEXT NOT NULL,
      triage_wait_time TEXT NOT NULL,
      suspected_diagnosis TEXT NOT NULL,
      icd10_code TEXT NOT NULL,
      emergency_exams_json TEXT NOT NULL,
      clinical_notes TEXT,
      evaluated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
      evaluator_nurse TEXT DEFAULT 'Növbətçi Triaq Həkimi / Tibb Bacısı'
    );

    CREATE TABLE IF NOT EXISTS emergency_alerts (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      code_type TEXT NOT NULL, -- 'CODE_BLUE', 'CODE_RED', 'CODE_YELLOW'
      patient_id INTEGER,
      ticket_number TEXT,
      patient_name TEXT,
      location TEXT NOT NULL,
      reason TEXT NOT NULL,
      status TEXT NOT NULL DEFAULT 'ACTIVE', -- 'ACTIVE', 'ACKNOWLEDGED', 'RESOLVED'
      ambulance_called INTEGER DEFAULT 0,
      doctor_notified INTEGER DEFAULT 0,
      created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
      acknowledged_at DATETIME,
      acknowledged_by TEXT
    );

    CREATE TABLE IF NOT EXISTS emergency_dispatches (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      alert_id INTEGER,
      dispatch_type TEXT NOT NULL, -- 'AMBULANCE_CALL', 'DOCTOR_PAGER', 'CRASH_CART'
      target_team TEXT NOT NULL,
      channel TEXT NOT NULL,
      status TEXT NOT NULL,
      message TEXT NOT NULL,
      created_at DATETIME DEFAULT CURRENT_TIMESTAMP
    );

    CREATE TABLE IF NOT EXISTS audit_logs (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      action_type TEXT NOT NULL,
      description TEXT NOT NULL,
      user_role TEXT NOT NULL DEFAULT 'Operator',
      created_at DATETIME DEFAULT CURRENT_TIMESTAMP
    );
  `);

  // Safe schema migrations for existing database
  try {
    db.exec(`ALTER TABLE emergency_alerts ADD COLUMN ambulance_called INTEGER DEFAULT 0;`);
  } catch {}
  try {
    db.exec(`ALTER TABLE emergency_alerts ADD COLUMN doctor_notified INTEGER DEFAULT 0;`);
  } catch {}
}

function seedInitialData(db: Database.Database) {
  // 1. Seed departments
  const insertDept = db.prepare(`
    INSERT OR IGNORE INTO departments (code, name, head_doctor, floor, active_cases)
    VALUES (?, ?, ?, ?, ?)
  `);

  insertDept.run("CARDIO", "Kardiologiya Şöbəsi", "Dr. Rəşad Məmmədov, PhD", "1-ci mərtəbə, Korpus A", 3);
  insertDept.run("EMERGENCY", "Təcili Yardım və Reanimasiya", "Dr. Kamran Əliyev", "1-ci mərtəbə, Şok Zalı", 2);
  insertDept.run("NEURO", "Nevrologiya və İnsult Mərkəzi", "Dr. Leyla Hüseynova", "2-ci mərtəbə, Korpus B", 1);
  insertDept.run("SURGERY", "Təcili Cərrahiyyə", "Dr. Elnur Qasımov", "2-ci mərtəbə, Əməliyyat Bloku", 2);
  insertDept.run("TRAUMA", "Travmatologiya və Ortopediya", "Dr. Fərid Nəzərov", "1-ci mərtəbə, Travma Bloku", 1);
  insertDept.run("INTERNAL", "Terapiya və Daxili Xəstəliklər", "Dr. Sevinc Quliyeva", "3-cü mərtəbə, Korpus A", 4);

  // 2. Seed cabinets
  const insertCabinet = db.prepare(`
    INSERT OR IGNORE INTO cabinets (cabinet_no, department, doctor_name, doctor_specialty, status, current_patient_ticket, floor, phone_ext)
    VALUES (?, ?, ?, ?, ?, ?, ?, ?)
  `);

  insertCabinet.run("KAB-101", "Kardiologiya Şöbəsi", "Dr. Rəşad Məmmədov", "İnvaziv Kardioloq", "Xəstə qəbulunda", "EMG-01", "Mərtəbə 1", "1011");
  insertCabinet.run("KAB-102", "Kardiologiya Şöbəsi", "Dr. Aygün Babayeva", "Klinik Kardioloq", "Boşdur", null, "Mərtəbə 1", "1012");
  insertCabinet.run("REA-01", "Təcili Yardım və Reanimasiya", "Dr. Kamran Əliyev", "Anestezioloq-Reanimatoloq", "Təcili çağırış", "EMG-02", "Şok Zalı", "1000");
  insertCabinet.run("REA-02", "Təcili Yardım və Reanimasiya", "Dr. Vüqar İsmayılov", "Təcili Tibbi Yardım Mütəxəssisi", "Boşdur", null, "Şok Zalı", "1001");
  insertCabinet.run("KAB-201", "Nevrologiya və İnsult Mərkəzi", "Dr. Leyla Hüseynova", "Nevropatoloq", "Xəstə qəbulunda", "T-103", "Mərtəbə 2", "2011");
  insertCabinet.run("CƏR-105", "Təcili Cərrahiyyə", "Dr. Elnur Qasımov", "Ümumi Cərrah", "Boşdur", null, "Mərtəbə 2", "2051");
  insertCabinet.run("TRAV-110", "Travmatologiya və Ortopediya", "Dr. Fərid Nəzərov", "Travmatoloq", "Boşdur", null, "Mərtəbə 1", "1101");
  insertCabinet.run("TER-301", "Terapiya və Daxili Xəstəliklər", "Dr. Sevinc Quliyeva", "Həkim-Terapevt", "Xəstə qəbulunda", "T-104", "Mərtəbə 3", "3011");

  // 3. Seed sample patients & triage records
  const samplePatients = [
    {
      ticket: "EMG-01",
      fin: "7A9B2C1",
      name: "Tofiq Əhmədov",
      birth: "1968-04-12",
      gender: "Kişi",
      phone: "+994 (50) 234-56-78",
      insurance: "İcbari Tibbi Sığorta",
      status: "Təxirəsalınmaz Reanimasiya",
      triage_level: 1,
      triage_color: "Qırmızı",
      dept: "Kardiologiya Şöbəsi",
      cabinet: "KAB-101",
      doctor: "Dr. Rəşad Məmmədov",
      vitals: {
        sys: 210,
        dia: 115,
        pulse: 128,
        spo2: 88,
        temp: 36.8,
        glucose: 7.2,
        vas: 9,
        symptoms: ["chest_pain_crushing", "pain_radiating_arm_jaw", "cold_clammy_sweat"],
        wait: "Dərhal (0 dəq)",
        dx: "Kəskin Koronar Sindrom (STEMI şübhəsi), Kəskin Sol Mədəcik Çatışmazlığı",
        icd: "I21.0",
        exams: ["EKQ 12-çıxarışlı (stat)", "Troponin I yüksək həssaslıq", "D-Dimer", "Döş qəfəsi Rentgeni", "Qanın Qazları (ABG)"],
        notes: "Xəstə təcili şok zalına yönləndirildi. İntensiv oksigenoterapiya və nitrat protokolu başladıldı.",
      },
    },
    {
      ticket: "EMG-02",
      fin: "5K1M8L9",
      name: "Aida Məhərrəmova",
      birth: "1954-11-23",
      gender: "Qadın",
      phone: "+994 (55) 789-12-34",
      insurance: "Özəl Sığorta",
      status: "Həkim qəbulunda",
      triage_level: 2,
      triage_color: "Narıncı",
      dept: "Nevrologiya və İnsult Mərkəzi",
      cabinet: "KAB-201",
      doctor: "Dr. Leyla Hüseynova",
      vitals: {
        sys: 185,
        dia: 105,
        pulse: 94,
        spo2: 95,
        temp: 37.1,
        glucose: 6.4,
        vas: 7,
        symptoms: ["facial_droop", "speech_difficulty", "unilateral_weakness"],
        wait: "10 dəqiqə ərzində",
        dx: "Kəskin Serebrovaskulyar Hadisə (İnsult şübhəsi, FAST+ protokolu)",
        icd: "I63.9",
        exams: ["Beyin KT Kontrassız (STAT)", "Beyin MRT Angioqrafiya", "Koaquloqramma (PT/INR, APTT)", "QÜA"],
        notes: "FAST protokolu müsbət. Trombolitik müalicə pəncərəsi yoxlanılır (son 2 saat ərzində başlayıb).",
      },
    },
    {
      ticket: "T-103",
      fin: "6J3X4Y2",
      name: "Cavid Quliyev",
      birth: "1985-08-19",
      gender: "Kişi",
      phone: "+994 (70) 555-88-99",
      insurance: "İcbari Tibbi Sığorta",
      status: "Gözləyir",
      triage_level: 3,
      triage_color: "Sarı",
      dept: "Təcili Cərrahiyyə",
      cabinet: "CƏR-105",
      doctor: "Dr. Elnur Qasımov",
      vitals: {
        sys: 135,
        dia: 85,
        pulse: 102,
        spo2: 98,
        temp: 38.6,
        glucose: 5.6,
        vas: 8,
        symptoms: ["severe_colic_pain", "board_rigid_abdomen"],
        wait: "60 dəqiqə ərzində",
        dx: "Kəskin Apandisit şübhəsi, Lokal Peritoneal Qıcıqlanma",
        icd: "K35.8",
        exams: ["Qarın Boşluğu USM", "Qanın Ümumi Analizi (leykositoz)", "Sidiyin Ümumi Analizi", "CRP"],
        notes: "Sağ qalça nahiyəsində kəskin ağrı və Blumberq simptomu müsbət. Cərrah baxışı təyin olundu.",
      },
    },
    {
      ticket: "T-104",
      fin: "9P4R7W1",
      name: "Nigar Vəliyeva",
      birth: "1992-02-14",
      gender: "Qadın",
      phone: "+994 (51) 444-22-11",
      insurance: "İcbari Tibbi Sığorta",
      status: "Gözləyir",
      triage_level: 4,
      triage_color: "Yaşıl",
      dept: "Terapiya və Daxili Xəstəliklər",
      cabinet: "TER-301",
      doctor: "Dr. Sevinc Quliyeva",
      vitals: {
        sys: 120,
        dia: 75,
        pulse: 78,
        spo2: 98,
        temp: 37.4,
        glucose: 5.1,
        vas: 3,
        symptoms: ["stridor_wheezing"],
        wait: "120 dəqiqə ərzində",
        dx: "Kəskin Bronxial İnfeksiya, Yuxarı Tənəffüs Yolları Katarı",
        icd: "J20.9",
        exams: ["Döş Qəfəsi Rentgenoqrafiyası", "QÜA"],
        notes: "Ümumi vəziyyəti stabildir, həyati təhlükə yoxdur. Standart növbəyə daxil edildi.",
      },
    },
    {
      ticket: "T-105",
      fin: "2D8E5F3",
      name: "Murad Həsənov",
      birth: "2001-09-30",
      gender: "Kişi",
      phone: "+994 (99) 333-11-22",
      insurance: "Nağd / Öz Hesabına",
      status: "Gözləyir",
      triage_level: 5,
      triage_color: "Mavi",
      dept: "Terapiya və Daxili Xəstəliklər",
      cabinet: "TER-301",
      doctor: "Dr. Sevinc Quliyeva",
      vitals: {
        sys: 118,
        dia: 78,
        pulse: 72,
        spo2: 99,
        temp: 36.6,
        glucose: 4.9,
        vas: 1,
        symptoms: [],
        wait: "Planlı / Təxirəsalına bilən",
        dx: "Planlı Profilaktik Tibbi Müayinə (Check-Up)",
        icd: "Z00.0",
        exams: ["Ümumi Qan və Sidik Analizləri", "Terapevt baxışı"],
        notes: "Şikayəti yoxdur. Sənədləşmə və planlı baxış üçün müraciət edib.",
      },
    },
  ];

  const insertPatient = db.prepare(`
    INSERT INTO patients (ticket_number, fin, full_name, birth_date, gender, phone, insurance_type, status, triage_level, triage_color, assigned_department, assigned_cabinet, assigned_doctor)
    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
  `);

  const insertTriage = db.prepare(`
    INSERT INTO triage_records (patient_id, sys, dia, pulse, spo2, temp, glucose, vas_pain, symptoms_json, triage_level, triage_color, triage_wait_time, suspected_diagnosis, icd10_code, emergency_exams_json, clinical_notes)
    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
  `);

  for (const p of samplePatients) {
    const res = insertPatient.run(
      p.ticket,
      p.fin,
      p.name,
      p.birth,
      p.gender,
      p.phone,
      p.insurance,
      p.status,
      p.triage_level,
      p.triage_color,
      p.dept,
      p.cabinet,
      p.doctor
    );
    const patientId = res.lastInsertRowid;

    insertTriage.run(
      patientId,
      p.vitals.sys,
      p.vitals.dia,
      p.vitals.pulse,
      p.vitals.spo2,
      p.vitals.temp,
      p.vitals.glucose,
      p.vitals.vas,
      JSON.stringify(p.vitals.symptoms),
      p.triage_level,
      p.triage_color,
      p.vitals.wait,
      p.vitals.dx,
      p.vitals.icd,
      JSON.stringify(p.vitals.exams),
      p.vitals.notes
    );
  }

  // 4. Seed initial Emergency Alert
  const insertAlert = db.prepare(`
    INSERT INTO emergency_alerts (code_type, patient_id, ticket_number, patient_name, location, reason, status)
    VALUES (?, ?, ?, ?, ?, ?, ?)
  `);

  insertAlert.run(
    "CODE_RED",
    1,
    "EMG-01",
    "Tofiq Əhmədov",
    "Kardiologiya / Şok Zalı",
    "Kritik Hipoksiya (SpO2: 88%) və Kəskin Koronar Sindrom (SYS: 210 mmHg)",
    "ACTIVE"
  );

  // 5. Seed Audit Log
  const insertLog = db.prepare(`
    INSERT INTO audit_logs (action_type, description, user_role)
    VALUES (?, ?, ?)
  `);
  insertLog.run("SYSTEM_INIT", "MedTriage Pro sistemi uğurla işə salındı və baza indeksləndi", "Sistem Administratoru");
  insertLog.run("TRIAGE_EVAL", "EMG-01 pasiyenti üçün Level 1 (Qırmızı) təcili triaq qərarı qəbul edildi", "Növbətçi Triaq Həkimi");
}
