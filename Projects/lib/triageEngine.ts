export interface VitalsInput {
  sys: number; // mmHg
  dia: number; // mmHg
  pulse: number; // bpm
  spo2: number; // %
  temp: number; // °C
  glucose: number; // mmol/L
  vasPain: number; // 0-10
  gcs?: number; // Glasgow Coma Scale 3-15
}

export interface VitalInterpretationItem {
  status: "normal" | "warning" | "critical";
  label: string;
  valueDisplay: string;
  clinicalRemark: string;
}

export interface VitalsAnalysis {
  sysInterpretation: VitalInterpretationItem;
  diaInterpretation: VitalInterpretationItem;
  pulseInterpretation: VitalInterpretationItem;
  spo2Interpretation: VitalInterpretationItem;
  tempInterpretation: VitalInterpretationItem;
  glucoseInterpretation: VitalInterpretationItem;
  vasInterpretation: VitalInterpretationItem;
  shockIndex: number; // Pulse / SYS
  shockIndexStatus: "normal" | "elevated" | "critical";
  shockIndexRemark: string;
}

export interface SymptomDefinition {
  id: string;
  category: "cardiovascular" | "respiratory" | "neurology" | "abdominal" | "trauma_shock";
  label: string;
  description: string;
  severityWeight: number; // 1-10
}

export const SYMPTOM_DEFINITIONS: SymptomDefinition[] = [
  // Ürək-damar
  {
    id: "chest_pain_crushing",
    category: "cardiovascular",
    label: "Döş qəfəsində kəskin sıxıcı / basıcı ağrı",
    description: "Retrosternal stenokardiya və ya kəskin infarkt riski",
    severityWeight: 9,
  },
  {
    id: "pain_radiating_arm_jaw",
    category: "cardiovascular",
    label: "Sol qola, çiyinə və ya alt çənəyə yayılan ağrı",
    description: "Kəskin işemiyanın tipik irradiasiyası",
    severityWeight: 8,
  },
  {
    id: "arrhythmia_palpitations",
    category: "cardiovascular",
    label: "Kəskin ürəkdöyünmə / ritm pozğunluğu (Aritmiya)",
    description: "Paroksizmal taxikardiya və ya fibrillyasiya riski",
    severityWeight: 7,
  },
  {
    id: "syncope_collapse",
    category: "cardiovascular",
    label: "Qəfil huşitirmə / sinkope epizodu",
    description: "Kardiogen kollaps və ya beyin perfuziyasının kritik çatışmazlığı",
    severityWeight: 8,
  },

  // Tənəffüs
  {
    id: "acute_dyspnea",
    category: "respiratory",
    label: "Kəskin boğulma / hava çatışmazlığı",
    description: "Asfiksiya, ağciyər ödemi və ya ağır bronxospazm",
    severityWeight: 9,
  },
  {
    id: "stridor_wheezing",
    category: "respiratory",
    label: "Stridor / küyülü xırıltılı tənəffüs",
    description: "Yuxarı tənəffüs yollarının stenozu / bronxial obstruksiya",
    severityWeight: 7,
  },
  {
    id: "cyanosis",
    category: "respiratory",
    label: "Sianoz / dodaq və barmaq uclarının göyərməsi",
    description: "Dərin toxuma hipoksiyası və deoksigenasiya",
    severityWeight: 9,
  },
  {
    id: "hemoptysis",
    category: "respiratory",
    label: "Qanhayxırma / bəlğəmdə təzə qan",
    description: "Ağciyər arteriyası tromboemboliyası (AATE) və ya destruksiya",
    severityWeight: 8,
  },

  // Nevrologiya (FAST)
  {
    id: "facial_droop",
    category: "neurology",
    label: "Üz asimmetriyası (bir tərəfin əyilməsi / sallanması)",
    description: "FAST protokolu: Facial Droop — Kəskin Serebrovaskulyar Qəza",
    severityWeight: 8,
  },
  {
    id: "speech_difficulty",
    category: "neurology",
    label: "Nitq itkisi və ya afaziya / dizartriya",
    description: "FAST protokolu: Speech Difficulty — Dominant yarımkürə zədələnməsi",
    severityWeight: 8,
  },
  {
    id: "unilateral_weakness",
    category: "neurology",
    label: "Qəfil birtərəfli zəiflik / əl-ayaq iflici (Hemiparez)",
    description: "FAST protokolu: Arm Weakness — Hərəki yolların kəskin işemiyası",
    severityWeight: 8,
  },
  {
    id: "seizure_convulsion",
    category: "neurology",
    label: "Qıcolma / konvulsiv epileptik tutma",
    description: "Epileptik status və ya kəllədaxili fəsad riski",
    severityWeight: 9,
  },
  {
    id: "severe_thunderclap_headache",
    category: "neurology",
    label: "Şimşəkvari kəskin dözülməz baş ağrısı",
    description: "Anevrizma yırtılması / Subaraxnoidal qansızma (SAQ) şübhəsi",
    severityWeight: 8,
  },

  // Qarın boşluğu
  {
    id: "board_rigid_abdomen",
    category: "abdominal",
    label: "Kəskin 'lövhəvari' qarın müdafiəsi (Defans)",
    description: "Kəskin peritonit və ya içi boş orqan perforasiyası",
    severityWeight: 9,
  },
  {
    id: "gastrointestinal_bleeding",
    category: "abdominal",
    label: "Qanaxma (Melena / qara nəcis və ya qanqusma)",
    description: "Yuxarı/aşağı mədə-bağırsaq kəskin qanitirməsi",
    severityWeight: 8,
  },
  {
    id: "severe_colic_pain",
    category: "abdominal",
    label: "Dözülməz kəskin sancıvari qarın ağrısı",
    description: "Kəskin apandisit, kəskin xolesistit və ya böyrək kolikası",
    severityWeight: 7,
  },

  // Travma / Şok
  {
    id: "unconscious_gcs_low",
    category: "trauma_shock",
    label: "Tam huşsuzluq / dərin sopor və ya koma",
    description: "Glazgo Şkalası (GCS) < 9, təxirəsalınmaz reanimasiya",
    severityWeight: 10,
  },
  {
    id: "cold_clammy_sweat",
    category: "trauma_shock",
    label: "Soyuq yapışqan tər / dəri solğunluğu (Şok)",
    description: "Periferik perfuziyanın kəskin azalması, dekompensasiya",
    severityWeight: 8,
  },
  {
    id: "severe_external_bleeding",
    category: "trauma_shock",
    label: "Kəskin massiv xarici qanitirmə",
    description: "Arterial/iri venoz qanitirmə, hemorragik şok təhlükəsi",
    severityWeight: 10,
  },
  {
    id: "polytrauma_fracture",
    category: "trauma_shock",
    label: "Açıq sınıq / politravma / kəllə-beyin travması",
    description: "Həyati təhlükəli anatomik destruksiya və qanitirmə",
    severityWeight: 9,
  },
];

export interface CDSSResult {
  triageLevel: 1 | 2 | 3 | 4 | 5;
  triageColor: "Qırmızı" | "Narıncı" | "Sarı" | "Yaşıl" | "Mavi";
  triageWaitTime: string;
  alertType: "CODE_BLUE" | "CODE_RED" | "CODE_YELLOW" | "NONE";
  emergencyTrigger: boolean;
  emergencyReason: string | null;
  suspectedDiagnosis: string;
  icd10Code: string;
  assignedDepartment: string;
  assignedCabinet: string;
  assignedSpecialist: string;
  emergencyExams: string[];
  matchedRules: string[];
  clinicalSummary: string;
  vitalsAnalysis: VitalsAnalysis;
}

export function analyzeVitals(vitals: VitalsInput): VitalsAnalysis {
  // Shock Index = Pulse / SYS (Normal: 0.5 - 0.7)
  const si = vitals.sys > 0 ? Number((vitals.pulse / vitals.sys).toFixed(2)) : 0;
  let siStatus: "normal" | "elevated" | "critical" = "normal";
  let siRemark = "Normal hemodinamik indeks (0.5 - 0.7). Qan dövranı stabildir.";

  if (si >= 1.0) {
    siStatus = "critical";
    siRemark = `Kritik Yüksək Şok İndeksi (${si} ≥ 1.0)! Gizli daxili qanitirmə, hipovolemiya və ya kardiogen şok təhlükəsi.`;
  } else if (si >= 0.8) {
    siStatus = "elevated";
    siRemark = `Subklinik Şok İndeksi (${si}). Taxikardiya və ya nisbi hipotenziya müşahidə olunur.`;
  }

  // SYS
  let sysStatus: "normal" | "warning" | "critical" = "normal";
  let sysRemark = "Normal sistolik təzyiq (90-139 mmHg)";
  if (vitals.sys >= 190) {
    sysStatus = "critical";
    sysRemark = "III Dərəcəli Kriz! İnsult və miokard zədələnməsi riski yüksəkdir.";
  } else if (vitals.sys >= 140) {
    sysStatus = "warning";
    sysRemark = "I-II Dərəcəli Arterial Hipertenziya.";
  } else if (vitals.sys < 85) {
    sysStatus = "critical";
    sysRemark = "Kritik Hipotenziya / Şok vəziyyəti (< 85 mmHg)!";
  } else if (vitals.sys < 95) {
    sysStatus = "warning";
    sysRemark = "Nisbi arterial hipotenziya.";
  }

  // DIA
  let diaStatus: "normal" | "warning" | "critical" = "normal";
  let diaRemark = "Normal diastolik təzyiq (60-89 mmHg)";
  if (vitals.dia >= 115) {
    diaStatus = "critical";
    diaRemark = "Kritik yüksək diastolik təzyiq (≥ 115 mmHg).";
  } else if (vitals.dia >= 90) {
    diaStatus = "warning";
    diaRemark = "Yüksək diastolik təzyiq.";
  } else if (vitals.dia < 50) {
    diaStatus = "critical";
    diaRemark = "Kritik aşağı diastolik təzyiq (< 50 mmHg).";
  }

  // Pulse
  let pulseStatus: "normal" | "warning" | "critical" = "normal";
  let pulseRemark = "Normal ürək ritmi (60-99 BPM)";
  if (vitals.pulse > 135) {
    pulseStatus = "critical";
    pulseRemark = "Kəskin Taxikardiya (> 135 BPM)! Ritm pozğunluğu və ya şok kompensasiyası.";
  } else if (vitals.pulse >= 100) {
    pulseStatus = "warning";
    pulseRemark = "Mülayim Taxikardiya.";
  } else if (vitals.pulse < 48) {
    pulseStatus = "critical";
    pulseRemark = "Kritik Bradikardiya (< 48 BPM)! Sinkope və AV-blokada təhlükəsi.";
  } else if (vitals.pulse < 60) {
    pulseStatus = "warning";
    pulseRemark = "Mülayim Bradikardiya.";
  }

  // SpO2
  let spo2Status: "normal" | "warning" | "critical" = "normal";
  let spo2Remark = "Normal oksigen saturasiyası (≥ 95%)";
  if (vitals.spo2 < 90) {
    spo2Status = "critical";
    spo2Remark = `Kritik Hipoksemiya (${vitals.spo2}% < 90%)! Dərhal O2 dəstəyi (10-15 L/dəq mask) tələb olunur.`;
  } else if (vitals.spo2 < 95) {
    spo2Status = "warning";
    spo2Remark = "Orta dərəcəli hipoksiya (90-94%).";
  }

  // Temp
  let tempStatus: "normal" | "warning" | "critical" = "normal";
  let tempRemark = "Normal bədən hərarəti (36.0 - 37.2 °C)";
  if (vitals.temp >= 39.4) {
    tempStatus = "critical";
    tempRemark = "Kritik Hipertermiya (≥ 39.4°C). Sepsis və ya MSS infeksiyası riski.";
  } else if (vitals.temp >= 37.3) {
    tempStatus = "warning";
    tempRemark = "Subfebril / Qızdırma halı.";
  } else if (vitals.temp < 35.0) {
    tempStatus = "critical";
    tempRemark = "Kritik Hipotermiya (< 35.0°C). Şok və periferik perfuziya pozulması.";
  }

  // Glucose
  let gluStatus: "normal" | "warning" | "critical" = "normal";
  let gluRemark = "Normal qan qlükozası (3.9 - 6.1 mmol/L)";
  if (vitals.glucose < 3.0) {
    gluStatus = "critical";
    gluRemark = `Kritik Hipoqlikemiya (${vitals.glucose} mmol/L)! Koma təhlükəsi, təcili 40% qlükoza infuziyası.`;
  } else if (vitals.glucose > 18.0) {
    gluStatus = "critical";
    gluRemark = `Kritik Hiperqlikemiya (${vitals.glucose} mmol/L)! Diabetik Ketoasidoz (DKA) / Hiperosmolyar koma riski.`;
  } else if (vitals.glucose > 10.0 || vitals.glucose < 3.8) {
    gluStatus = "warning";
    gluRemark = "Normadan kənar qlükoza səviyyəsi.";
  }

  // VAS Pain
  let vasStatus: "normal" | "warning" | "critical" = "normal";
  let vasRemark = "Ağrısız və ya yüngül narahatlıq (0-3)";
  if (vitals.vasPain >= 8) {
    vasStatus = "critical";
    vasRemark = "Dözülməz şiddətli kəskin ağrı (VAS 8-10). Təcili analgetik protokolu.";
  } else if (vitals.vasPain >= 4) {
    vasStatus = "warning";
    vasRemark = "Orta dərəcəli kəskin ağrı (VAS 4-7).";
  }

  return {
    sysInterpretation: { status: sysStatus, label: "SYS", valueDisplay: `${vitals.sys} mmHg`, clinicalRemark: sysRemark },
    diaInterpretation: { status: diaStatus, label: "DIA", valueDisplay: `${vitals.dia} mmHg`, clinicalRemark: diaRemark },
    pulseInterpretation: { status: pulseStatus, label: "Nəbz", valueDisplay: `${vitals.pulse} bpm`, clinicalRemark: pulseRemark },
    spo2Interpretation: { status: spo2Status, label: "SpO2", valueDisplay: `${vitals.spo2}%`, clinicalRemark: spo2Remark },
    tempInterpretation: { status: tempStatus, label: "Hərarət", valueDisplay: `${vitals.temp} °C`, clinicalRemark: tempRemark },
    glucoseInterpretation: { status: gluStatus, label: "Qlükoza", valueDisplay: `${vitals.glucose} mmol/L`, clinicalRemark: gluRemark },
    vasInterpretation: { status: vasStatus, label: "VAS Ağrı", valueDisplay: `${vitals.vasPain}/10`, clinicalRemark: vasRemark },
    shockIndex: si,
    shockIndexStatus: siStatus,
    shockIndexRemark: siRemark,
  };
}

export function evaluateTriageCDSS(vitals: VitalsInput, symptoms: string[]): CDSSResult {
  const matchedRules: string[] = [];
  const sSet = new Set(symptoms);
  const vitalsAnalysis = analyzeVitals(vitals);

  const isHypoxicCritical = vitals.spo2 < 90;
  const isHypoxicModerate = vitals.spo2 >= 90 && vitals.spo2 < 95;
  const isHypotensive = vitals.sys < 90;
  const isSevereHypotensive = vitals.sys < 75;
  const isSevereHypertensive = vitals.sys >= 190 || vitals.dia >= 115;
  const isExtremeHypertensive = vitals.sys >= 220 || vitals.dia >= 125;
  const isSevereTachycardia = vitals.pulse > 135;
  const isSevereBradycardia = vitals.pulse < 48;
  const isHypoglycemic = vitals.glucose < 3.0;
  const isHyperglycemicSevere = vitals.glucose > 18.0;
  const isHighFever = vitals.temp >= 39.4;
  const isSevereHypothermia = vitals.temp < 35.0;
  const isSeverePain = vitals.vasPain >= 8;
  const isCriticalShockIndex = vitalsAnalysis.shockIndex >= 1.0;

  // RULE 1: Kəskin Tənəffüs Kollapsı & Kritik Hipoksiya (Asfiksiya / ARDS)
  if (isHypoxicCritical || (sSet.has("acute_dyspnea") && (sSet.has("stridor_wheezing") || sSet.has("cyanosis")))) {
    matchedRules.push("Qayda 1: Kritik Hipoksiya / Kəskin Tənəffüs Çatışmazlığı (MTS L1)");
    return {
      triageLevel: 1,
      triageColor: "Qırmızı",
      triageWaitTime: "Dərhal (0 dəqiqə)",
      alertType: "CODE_BLUE",
      emergencyTrigger: true,
      emergencyReason: `SpO2 kritik dərəcədə aşağıdır (${vitals.spo2}%) və ya asfiksiya əlamətləri mövcuddur`,
      suspectedDiagnosis: "Kəskin Tənəffüs Çatışmazlığı (Tip I/II), Asfiksiya sindromu",
      icd10Code: "J96.0",
      assignedDepartment: "Təcili Yardım və Reanimasiya",
      assignedCabinet: "REA-01",
      assignedSpecialist: "Reanimatoloq-Anestezioloq",
      emergencyExams: [
        "Qanın Qazları Analizi (ABG - Stat)",
        "Təcili İntubasiya və CPAP/BIPAP Hazırlığı",
        "EKQ 12-çıxarışlı (stat)",
        "Döş Qəfəsi Portativ Rentgeni",
        "D-Dimer və Troponin T",
      ],
      matchedRules,
      clinicalSummary: "Pasiyentdə həyati vacib oksigenasiya pozğunluğu qeydə alınıb. Reanimasiya qrupu dərhal çağırılmalıdır.",
      vitalsAnalysis,
    };
  }

  // RULE 2: Tam Huşsuzluq və ya Dərin Şok vəziyyəti
  if (
    sSet.has("unconscious_gcs_low") ||
    (isSevereHypotensive && (sSet.has("cold_clammy_sweat") || sSet.has("syncope_collapse"))) ||
    (isCriticalShockIndex && (sSet.has("cold_clammy_sweat") || isHypotensive))
  ) {
    matchedRules.push("Qayda 2: Huşsuzluq və ya Dekompensasiya Olunmuş Şok vəziyyəti (MTS L1)");
    return {
      triageLevel: 1,
      triageColor: "Qırmızı",
      triageWaitTime: "Dərhal (0 dəqiqə)",
      alertType: "CODE_BLUE",
      emergencyTrigger: true,
      emergencyReason: `Tam huşsuzluq və ya kəskin hipotenziya (SYS: ${vitals.sys} mmHg, Şok İndeksi: ${vitalsAnalysis.shockIndex})`,
      suspectedDiagnosis: "Kəskin Şok Vəziyyəti (Kardiogen / Septik / Hipovolemik), Koma",
      icd10Code: "R57.9",
      assignedDepartment: "Təcili Yardım və Reanimasiya",
      assignedCabinet: "REA-01",
      assignedSpecialist: "Reanimatoloq-Anestezioloq",
      emergencyExams: [
        "Mərkəzi Venoz Kateterizasiya və İnfuziya",
        "EKQ və Daimi Hemodinamik Monitorinq",
        "Qanda Laktat, Elektrolitlər və Qazlar",
        "Təcili Qlükoza və Toksikoloji Skrininq",
        "Reanimasiya Protokolu",
      ],
      matchedRules,
      clinicalSummary: "Pasiyentin şüuru qapalıdır və ya dərin hemodinamik kollaps mövcuddur. Reanimasiya zalına yerləşdirilməlidir.",
      vitalsAnalysis,
    };
  }

  // RULE 3: Massiv Xarici Qanitirmə və ya Ağır Politravma
  if (sSet.has("severe_external_bleeding") || (sSet.has("polytrauma_fracture") && (isHypotensive || isSevereTachycardia || isCriticalShockIndex))) {
    matchedRules.push("Qayda 3: Massiv Qanaxma və Ağır Politravma (MTS L1)");
    return {
      triageLevel: 1,
      triageColor: "Qırmızı",
      triageWaitTime: "Dərhal (0 dəqiqə)",
      alertType: "CODE_RED",
      emergencyTrigger: true,
      emergencyReason: "Kəskin massiv qanitirmə və hemodinamik qeyri-stabillik (Şok indeksi yüksək)",
      suspectedDiagnosis: "Hemorragik Şok, Massiv Travmatik Qanaxma, Politravma",
      icd10Code: "T07 / R57.1",
      assignedDepartment: "Təcili Cərrahiyyə",
      assignedCabinet: "CƏR-105",
      assignedSpecialist: "Ümumi Cərrah & Travmatoloq",
      emergencyExams: [
        "Turniket və Cərrahi Hemostaz",
        "Təcili Qan Qrupu və Çarpaz Uyğunlaşma (Stat Cross-match)",
        "Koaquloqramma, Hemoglobin və Hematokrit",
        "FAST Protokolu ilə Qarın Boşluğu USM",
        "Politravma Kompüter Tomoqrafiyası (Pan-CT)",
      ],
      matchedRules,
      clinicalSummary: "Massiv qanaxmanın cərrahi dayandırılması və təcili hemotransfuziya tələb olunur.",
      vitalsAnalysis,
    };
  }

  // RULE 4: Kəskin Miokard İnfarktı / Kəskin Koronar Sindrom (STEMI)
  if (
    sSet.has("chest_pain_crushing") &&
    (sSet.has("pain_radiating_arm_jaw") || sSet.has("cold_clammy_sweat") || isSevereHypertensive || isHypotensive)
  ) {
    matchedRules.push("Qayda 4: Kəskin Koronar Sindrom / Miokard İnfarktı Şübhəsi (MTS L1)");
    return {
      triageLevel: 1,
      triageColor: "Qırmızı",
      triageWaitTime: "Dərhal (0 dəqiqə)",
      alertType: "CODE_RED",
      emergencyTrigger: true,
      emergencyReason: "Döş qəfəsində sıxıcı ağrı və irradiasiya ilə kəskin işemiya mənzərəsi",
      suspectedDiagnosis: "Kəskin Koronar Sindrom (STEMI / NSTEMI şübhəsi)",
      icd10Code: "I21.9",
      assignedDepartment: "Kardiologiya Şöbəsi",
      assignedCabinet: "KAB-101",
      assignedSpecialist: "İnvaziv Kardioloq",
      emergencyExams: [
        "12-çıxarışlı EKQ (10 dəqiqə ərzində stat)",
        "Yüksək Həssaslıqlı Troponin I / T",
        "CK-MB, D-Dimer, Koaquloqramma",
        "Təcili Exokardioqrafiya (ECHO)",
        "Koronar Angioqrafiya (Koro-KT) hazırlığı",
      ],
      matchedRules,
      clinicalSummary: "Tipik stenokardik ağrı və risk faktorları. STAT EKQ və kardiomarkerlər tələb olunur.",
      vitalsAnalysis,
    };
  }

  // RULE 5: Kəskin Serebrovaskulyar Hadisə / İnsult (FAST Protokolu Müsbət)
  if (
    (sSet.has("facial_droop") && sSet.has("speech_difficulty")) ||
    (sSet.has("unilateral_weakness") && (sSet.has("facial_droop") || sSet.has("speech_difficulty")))
  ) {
    matchedRules.push("Qayda 5: FAST+ Kəskin Serebrovaskulyar İnsult (MTS L1 Fast Track)");
    return {
      triageLevel: 1,
      triageColor: "Qırmızı",
      triageWaitTime: "10 dəqiqə ərzində (Trombolitik Pəncərə)",
      alertType: "CODE_RED",
      emergencyTrigger: true,
      emergencyReason: "FAST protokolu müsbətdir: Üz əyilməsi, nitq pozulması və birtərəfli hemiparez",
      suspectedDiagnosis: "Kəskin Serebrovaskulyar Qəza (İşemik İnsult şübhəsi, FAST+)",
      icd10Code: "I63.9",
      assignedDepartment: "Nevrologiya və İnsult Mərkəzi",
      assignedCabinet: "KAB-201",
      assignedSpecialist: "Nevroloq (İnsult Komandası)",
      emergencyExams: [
        "Kəllə Beyin Kontrassız KT (STAT, 20 dəqiqə ərzində)",
        "Beyin Damarlarının KT-Angioqrafiyası",
        "Qanda Qlükoza və Koaquloqramma (INR, APTT)",
        "NIHSS Nevroloji Şkalası üzrə qiymətləndirmə",
        "Trombolitik Terapiya (r-tPA) Hazırlığı",
      ],
      matchedRules,
      clinicalSummary: "Trombolizis pəncərəsi üçün təcili 'Code Stroke' aktivləşdirilməli və KT-yə aparılmalıdır.",
      vitalsAnalysis,
    };
  }

  // RULE 6: Kəskin Peritonit və Daxili Orqan Perforasiyası ("Cərrahi Qarın")
  if (sSet.has("board_rigid_abdomen") || (sSet.has("gastrointestinal_bleeding") && (isSevereTachycardia || isHypotensive))) {
    matchedRules.push("Qayda 6: Kəskin Peritonit / Cərrahi Qarın Sindromu (MTS L1/L2)");
    return {
      triageLevel: 1,
      triageColor: "Qırmızı",
      triageWaitTime: "Dərhal (0 dəqiqə)",
      alertType: "CODE_RED",
      emergencyTrigger: true,
      emergencyReason: "Lövhəvari qarın müdafiəsi və ya daxili qanaxma ilə şok riski",
      suspectedDiagnosis: "Kəskin Peritonit, İçi Boş Orqan Perforasiyası, Daxili Qanaxma",
      icd10Code: "K65.0",
      assignedDepartment: "Təcili Cərrahiyyə",
      assignedCabinet: "CƏR-105",
      assignedSpecialist: "Ümumi Cərrah",
      emergencyExams: [
        "Qarın Boşluğunun İcmal Rentgeni (Sərbəst qaz təyini)",
        "Qarın Boşluğu Təcili USM (FAST)",
        "QÜA (Leykositar formula və Çubuqnüvəli meyl)",
        "Qan Qrupu və Rh-faktor",
        "Təcili Laparotomiya hazırlığı",
      ],
      matchedRules,
      clinicalSummary: "Kəskin peritonit əlamətləri. Cərrahi komanda təxirəsalınmaz baxış keçirməlidir.",
      vitalsAnalysis,
    };
  }

  // RULE 7: Ağır Hipoqlikemik Kriz
  if (isHypoglycemic || (vitals.glucose < 3.5 && (sSet.has("cold_clammy_sweat") || sSet.has("syncope_collapse")))) {
    matchedRules.push("Qayda 7: Kəskin Hipoqlikemik Vəziyyət (MTS L2)");
    return {
      triageLevel: 2,
      triageColor: "Narıncı",
      triageWaitTime: "10 dəqiqə ərzində",
      alertType: "CODE_YELLOW",
      emergencyTrigger: true,
      emergencyReason: `Kritik aşağı qlükoza göstəricisi: ${vitals.glucose} mmol/L (koma təhlükəsi)`,
      suspectedDiagnosis: "Kəskin Hipoqlikemiya, Hipoqlikemik Koma Önü",
      icd10Code: "E15",
      assignedDepartment: "Terapiya və Daxili Xəstəliklər",
      assignedCabinet: "TER-301",
      assignedSpecialist: "Endokrinoloq / Terapevt",
      emergencyExams: [
        "Təkrari Qlükomandant Təyini",
        "40%-li Qlükoza Məhlulunun Vena Daxili Yeridilməsi",
        "Qanda Qlükoza Dinamikası",
        "Qan Elektrolitləri",
      ],
      matchedRules,
      clinicalSummary: "Beyin qlikopeniyasını aradan qaldırmaq üçün təcili hipertonik qlükoza infuziyası lazımdır.",
      vitalsAnalysis,
    };
  }

  // RULE 8: Hipertonik Kriz və ya Beyin Ensefalopatiyası
  if (isExtremeHypertensive || (isSevereHypertensive && (sSet.has("severe_thunderclap_headache") || vitals.vasPain >= 8))) {
    matchedRules.push("Qayda 8: Komplikasiyalı Hipertonik Kriz (MTS L2)");
    return {
      triageLevel: 2,
      triageColor: "Narıncı",
      triageWaitTime: "10 dəqiqə ərzində",
      alertType: "CODE_YELLOW",
      emergencyTrigger: true,
      emergencyReason: `Kəskin arterial hipertenziya (SYS: ${vitals.sys} / DIA: ${vitals.dia} mmHg) və hədəf orqan zədələnməsi riski`,
      suspectedDiagnosis: "Komplikasiyalı Hipertonik Kriz, Hipertenziv Ensefalopatiya",
      icd10Code: "I10",
      assignedDepartment: "Kardiologiya Şöbəsi",
      assignedCabinet: "KAB-101",
      assignedSpecialist: "Kardioloq",
      emergencyExams: [
        "Parenteral Antihipertenziv Terapiya Protokolu",
        "EKQ 12-çıxarışlı",
        "Göz Dibi Müayinəsi (Oftalmoskopiya)",
        "Qanda Kreatinin, Karbamid və Elektrolitlər",
        "Beyin KT (baş ağrısı şiddətlidirsə)",
      ],
      matchedRules,
      clinicalSummary: "Təzyiqin hədəf orqanları zədələməməsi üçün tədricən və nəzarət altında endirilməsi lazımdır.",
      vitalsAnalysis,
    };
  }

  // RULE 9: Qıcolma Tutması və ya Şimşəkvari Baş Ağrısı (SAQ Şübhəsi)
  if (sSet.has("seizure_convulsion") || sSet.has("severe_thunderclap_headache")) {
    matchedRules.push("Qayda 9: Konvulsiv Sindrom / Subaraxnoidal Qansızma Şübhəsi (MTS L2)");
    return {
      triageLevel: 2,
      triageColor: "Narıncı",
      triageWaitTime: "10 dəqiqə ərzində",
      alertType: "CODE_YELLOW",
      emergencyTrigger: true,
      emergencyReason: "Qıcolma statusu və ya kəskin kəllədaxili fəlakət şübhəsi",
      suspectedDiagnosis: "Epileptik Tutma / Subaraxnoidal Qansızma (SAQ) Şübhəsi",
      icd10Code: "G40.9 / I60.9",
      assignedDepartment: "Nevrologiya və İnsult Mərkəzi",
      assignedCabinet: "KAB-201",
      assignedSpecialist: "Nevroloq",
      emergencyExams: [
        "Beyin KT / KT-Angioqrafiya",
        "Elektroensefaloqrafiya (EEQ)",
        "Antikonvulsant Terapiya",
        "Lyumbal Punksiyaya Göstərişin Qiymətləndirilməsi",
      ],
      matchedRules,
      clinicalSummary: "Kəskin nevroloji təxirəsalınmaz vəziyyət. Beyin qanaxması istisna edilməlidir.",
      vitalsAnalysis,
    };
  }

  // RULE 10: Ağır Aritmiya / Paroksizmal Taxikardiya və ya Kritik Bradikardiya
  if (
    (isSevereTachycardia || isSevereBradycardia) &&
    (sSet.has("arrhythmia_palpitations") || sSet.has("syncope_collapse") || sSet.has("chest_pain_crushing"))
  ) {
    matchedRules.push("Qayda 10: Hemodinamik Təsiri Olan Ağır Aritmiya (MTS L2)");
    return {
      triageLevel: 2,
      triageColor: "Narıncı",
      triageWaitTime: "10 dəqiqə ərzində",
      alertType: "CODE_YELLOW",
      emergencyTrigger: true,
      emergencyReason: `Kritik nəbz anomaliyası (${vitals.pulse} bpm) və ritm pozulması əlamətləri`,
      suspectedDiagnosis: "Paroksizmal Mədəciküstü/Mədəcik Taxikardiyası, AV Bloka",
      icd10Code: "I47.9",
      assignedDepartment: "Kardiologiya Şöbəsi",
      assignedCabinet: "KAB-101",
      assignedSpecialist: "Aritmoloq-Kardioloq",
      emergencyExams: [
        "EKQ Monitorinq və Ritm Zolağının Yazılması",
        "Qanda Kalium, Maqnezium və Kalsium Səviyyəsi",
        "Defibrillyator / Kardioversiya Hazırlığı",
        "Anti-aritmik Dərman Protokolu",
      ],
      matchedRules,
      clinicalSummary: "Malign aritmiya təhlükəsi. Monitorizasiya və kardioloq müdaxiləsi vacibdir.",
      vitalsAnalysis,
    };
  }

  // RULE 11: Kəskin Bronxospazm və ya Ağır Bronxial Astma Tutması
  if ((sSet.has("stridor_wheezing") || sSet.has("acute_dyspnea")) && isHypoxicModerate) {
    matchedRules.push("Qayda 11: Kəskin Bronxospazm / Astma Tutması (MTS L2)");
    return {
      triageLevel: 2,
      triageColor: "Narıncı",
      triageWaitTime: "10 dəqiqə ərzində",
      alertType: "CODE_YELLOW",
      emergencyTrigger: false,
      emergencyReason: null,
      suspectedDiagnosis: "Kəskin Astma Tutması, Ağır Bronxospazm Obstruksiyası",
      icd10Code: "J45.9",
      assignedDepartment: "Terapiya və Daxili Xəstəliklər",
      assignedCabinet: "TER-301",
      assignedSpecialist: "Pulmonoloq / Terapevt",
      emergencyExams: [
        "Nebulayzerlə Bronxodilatator Terapiyası (Salbutamol + İpratropium)",
        "Sistem Kortikosteroid İnfusion",
        "Nəmləndirilmiş Oksigen Dəstəyi",
        "SpO2 və Tənəffüs Sayı Dinamik Nəzarəti",
      ],
      matchedRules,
      clinicalSummary: "Bronxial keçiricilik pozulub, oksigenasiya düşməyə meyllidir.",
      vitalsAnalysis,
    };
  }

  // RULE 12: Kəskin Diabetik Ketoasidoz (DKA) / Hiperosmolyar Koma Riski
  if (isHyperglycemicSevere) {
    matchedRules.push("Qayda 12: Kritik Hiperqlikemiya / Diabetik Ketoasidoz Şübhəsi (MTS L2)");
    return {
      triageLevel: 2,
      triageColor: "Narıncı",
      triageWaitTime: "10 dəqiqə ərzində",
      alertType: "CODE_YELLOW",
      emergencyTrigger: false,
      emergencyReason: null,
      suspectedDiagnosis: "Diabetik Ketoasidoz (DKA), Hiperqlikemik Sindrom",
      icd10Code: "E10.1",
      assignedDepartment: "Terapiya və Daxili Xəstəliklər",
      assignedCabinet: "TER-301",
      assignedSpecialist: "Endokrinoloq",
      emergencyExams: [
        "Sidikdə və Qanda Keton Cisimlərinin Təyini",
        "Qan Qazları və Qələvi-Turşu Müvazinəti (pH, HCO3)",
        "İntensiv İnfuzion Rehidrasiya",
        "Qısa Təsirli İnsulin Protokolu",
      ],
      matchedRules,
      clinicalSummary: "Kritik hiperqlikemiya orqanizmin kəskin susuzlaşması və ketoasidoza səbəb ola bilər.",
      vitalsAnalysis,
    };
  }

  // RULE 13: Kəskin Qarın Ağrısı / Apandisit / Renal Kolik (VAS >= 7)
  if (sSet.has("severe_colic_pain") || (isSeverePain && sSet.has("board_rigid_abdomen"))) {
    matchedRules.push("Qayda 13: Kəskin Sancıvari Qarın Ağrısı / Cərrahi Şübhə (MTS L3)");
    return {
      triageLevel: 3,
      triageColor: "Sarı",
      triageWaitTime: "60 dəqiqə ərzində",
      alertType: "NONE",
      emergencyTrigger: false,
      emergencyReason: null,
      suspectedDiagnosis: "Kəskin Apandisit / Kəskin Xolesistit / Böyrək Sancısı",
      icd10Code: "K35.8",
      assignedDepartment: "Təcili Cərrahiyyə",
      assignedCabinet: "CƏR-105",
      assignedSpecialist: "Ümumi Cərrah",
      emergencyExams: [
        "Qarın Boşluğu və Retroperitoneal Sahənin USM-i",
        "Qanın Ümumi Analizi (QÜA + Leykositar düstur)",
        "Sidiyin Ümumi Analizi (Hematuriya / leykosituriya)",
        "CRP və Biokimyəvi Profil",
      ],
      matchedRules,
      clinicalSummary: "Ağrı intensivdir, cərrahi və ya uroloji patologiya üçün differensiasiya edilməlidir.",
      vitalsAnalysis,
    };
  }

  // RULE 14: Kəskin Qızdırma və Sistem İnfeksiyası (Sepsis Şübhəsi)
  if ((isHighFever || isSevereHypothermia) && (vitals.pulse > 100 || sSet.has("acute_dyspnea"))) {
    matchedRules.push("Qayda 14: Sistem İltihabi Cavab Sindromu (SIRS / Sepsis Şübhəsi) (MTS L3)");
    return {
      triageLevel: 3,
      triageColor: "Sarı",
      triageWaitTime: "60 dəqiqə ərzində",
      alertType: "NONE",
      emergencyTrigger: false,
      emergencyReason: null,
      suspectedDiagnosis: "Yüksək Qızdırmalı İnfeksion Proses, Sepsis Skrininqi",
      icd10Code: "R50.9 / A41.9",
      assignedDepartment: "Terapiya və Daxili Xəstəliklər",
      assignedCabinet: "TER-301",
      assignedSpecialist: "İnfeksionist / Terapevt",
      emergencyExams: [
        "Hemokultura (Qan əkilməsi 2 nöqtədən)",
        "Prokalsitonin və C-Reaktiv Zülal (CRP)",
        "QÜA və Sidik Analizi",
        "Rentgenoqrafiya (Ağciyər infeksiyası şübhəsi varsa)",
      ],
      matchedRules,
      clinicalSummary: "Temperatur kritikdir, taxikardiya mövcuddur. İnfeksiya ocağı dərhal təyin edilməlidir.",
      vitalsAnalysis,
    };
  }

  // RULE 15: Travmatoloji Zədələnmə (Sınıq / Çıxıq / Yumşaq Toxuma Travması)
  if (sSet.has("polytrauma_fracture") || (vitals.vasPain >= 6 && sSet.has("severe_colic_pain") === false)) {
    matchedRules.push("Qayda 15: Qapalı Travma və Ya Ətraf Sınığı Şübhəsi (MTS L4)");
    return {
      triageLevel: 4,
      triageColor: "Yaşıl",
      triageWaitTime: "120 dəqiqə ərzində",
      alertType: "NONE",
      emergencyTrigger: false,
      emergencyReason: null,
      suspectedDiagnosis: "Ətraf Zədələnməsi, Bağ Aparatının Qopması və ya Sınıq Şübhəsi",
      icd10Code: "S82.9",
      assignedDepartment: "Travmatologiya və Ortopediya",
      assignedCabinet: "TRAV-110",
      assignedSpecialist: "Həkim-Travmatoloq",
      emergencyExams: [
        "2 Proyeksiyada Zədələnmiş Nahiyənin Rentgenoqrafiyası",
        "İmmobilizasiya (Gips və ya şina sarğısı)",
        "Ağrıkəsici Terapiya",
      ],
      matchedRules,
      clinicalSummary: "Həyati təhlükə yoxdur, sümük-əzələ sistemi travmasının rentgen diaqnostikası tələb olunur.",
      vitalsAnalysis,
    };
  }

  // RULE 16: Yüngül Kəskin Respirator və ya Terapevtik Narahatlıq (MTS L4 Yaşıl)
  if (symptoms.length > 0 || vitals.vasPain >= 4 || vitals.temp >= 37.5) {
    matchedRules.push("Qayda 16: Yüngül / Orta Dərəcəli Təxirəsalına Bilən Narahatlıq (MTS L4)");
    return {
      triageLevel: 4,
      triageColor: "Yaşıl",
      triageWaitTime: "120 dəqiqə ərzində",
      alertType: "NONE",
      emergencyTrigger: false,
      emergencyReason: null,
      suspectedDiagnosis: "Kəskin Respirator Virus İnfeksiyası (KRVİ) / Əzələ Ağrısı",
      icd10Code: "J06.9",
      assignedDepartment: "Terapiya və Daxili Xəstəliklər",
      assignedCabinet: "TER-301",
      assignedSpecialist: "Ümumi Terapevt",
      emergencyExams: ["Ümumi Qan Analizi (QÜA)", "Həkim-Terapevtin Rutin Baxışı"],
      matchedRules,
      clinicalSummary: "Pasiyentin həyati göstəriciləri kompensasiya olunub. Standart növbə qəbulu uyğundur.",
      vitalsAnalysis,
    };
  }

  // RULE 17: Tam Normal Göstəricilər və Planlı Müraciət (MTS L5 Mavi)
  matchedRules.push("Qayda 17: Normal Vital Göstəricilər / Planlı Profilaktik Müraciət (MTS L5)");
  return {
    triageLevel: 5,
    triageColor: "Mavi",
    triageWaitTime: "Planlı / Təxirəsalına bilən",
    alertType: "NONE",
    emergencyTrigger: false,
    emergencyReason: null,
    suspectedDiagnosis: "Profilaktik Tibbi Müayinə / Planlı Məsləhət (Check-Up)",
    icd10Code: "Z00.0",
    assignedDepartment: "Terapiya və Daxili Xəstəliklər",
    assignedCabinet: "TER-301",
    assignedSpecialist: "Ümumi Terapevt",
    emergencyExams: ["Rutin Qan və Sidik Müayinəsi", "Profilaktik Məsləhət"],
    matchedRules,
    clinicalSummary: "Şikayəti və ya kəskin patologiyası yoxdur. Planlı rejimdə qəbul tövsiyə edilir.",
    vitalsAnalysis,
  };
}
