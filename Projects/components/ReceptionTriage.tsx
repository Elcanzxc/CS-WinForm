"use client";

import React, { useState, useEffect } from "react";
import {
  Activity,
  Heart,
  Thermometer,
  Wind,
  Droplet,
  Zap,
  CheckSquare,
  Square,
  AlertTriangle,
  User,
  Phone,
  Calendar,
  CreditCard,
  ShieldAlert,
  Send,
  Printer,
  Sparkles,
  Info,
  Stethoscope,
  Building,
  AlertCircle,
  HelpCircle,
  ChevronRight,
  ChevronLeft,
  CheckCircle2,
  SlidersHorizontal,
  LayoutGrid,
} from "lucide-react";
import {
  VitalsInput,
  SYMPTOM_DEFINITIONS,
  evaluateTriageCDSS,
  CDSSResult,
  analyzeVitals,
} from "@/lib/triageEngine";
import { playEmergencyAlert } from "@/lib/audioAlerts";
import { PrintableTicketData } from "./PrintReferralTicket";

interface ReceptionTriageProps {
  onPatientCreated: (ticketData: PrintableTicketData) => void;
  onEmergencyTriggered: () => void;
}

export default function ReceptionTriage({
  onPatientCreated,
  onEmergencyTriggered,
}: ReceptionTriageProps) {
  // Navigation mode: 'stepped' (guided, calm, low-cognitive-load) vs 'all' (wide cockpit)
  const [viewMode, setViewMode] = useState<"stepped" | "all">("stepped");
  const [activeStep, setActiveStep] = useState<1 | 2 | 3>(1);

  // Active Symptom Category Tab
  const [activeSymptomCat, setActiveSymptomCat] = useState<
    "cardiovascular" | "respiratory" | "neurology" | "abdominal" | "trauma_shock"
  >("cardiovascular");

  // Form State
  const [fin, setFin] = useState("");
  const [fullName, setFullName] = useState("");
  const [birthDate, setBirthDate] = useState("1982-06-14");
  const [gender, setGender] = useState("Kişi");
  const [phone, setPhone] = useState("+994 (50) 234-56-78");
  const [insuranceType, setInsuranceType] = useState("İcbari Tibbi Sığorta");
  const [allergies, setAllergies] = useState<string>("Qeyd edilməyib");
  const [clinicalNotes, setClinicalNotes] = useState("");
  const [calculatedAge, setCalculatedAge] = useState("-");

  // Vitals State
  const [vitals, setVitals] = useState<VitalsInput>({
    sys: 120,
    dia: 80,
    pulse: 76,
    spo2: 98,
    temp: 36.6,
    glucose: 5.2,
    vasPain: 2,
    gcs: 15,
  });

  // Selected Symptoms
  const [selectedSymptoms, setSelectedSymptoms] = useState<string[]>([]);
  const [submitting, setSubmitting] = useState(false);
  const [statusMessage, setStatusMessage] = useState<{ type: "success" | "error"; text: string } | null>(null);

  // Live CDSS Calculation
  const [cdssPreview, setCdssPreview] = useState<CDSSResult>(() =>
    evaluateTriageCDSS(vitals, selectedSymptoms)
  );

  useEffect(() => {
    const result = evaluateTriageCDSS(vitals, selectedSymptoms);
    setCdssPreview(result);
  }, [vitals, selectedSymptoms]);

  useEffect(() => {
    try {
      const birth = new Date(birthDate);
      const now = new Date();
      let age = now.getFullYear() - birth.getFullYear();
      const m = now.getMonth() - birth.getMonth();
      if (m < 0 || (m === 0 && now.getDate() < birth.getDate())) {
        age--;
      }
      setCalculatedAge(isNaN(age) || age < 0 ? "-" : `${age} yaş`);
    } catch {
      setCalculatedAge("-");
    }
  }, [birthDate]);

  const toggleSymptom = (id: string) => {
    setSelectedSymptoms((prev) =>
      prev.includes(id) ? prev.filter((s) => s !== id) : [...prev, id]
    );
  };

  const loadPreset = (preset: "STEMI" | "STROKE" | "PERITONITIS" | "NORMAL") => {
    if (preset === "STEMI") {
      setFullName("Qurban Səfərov");
      setFin("7T4M9R1");
      setBirthDate("1964-07-12");
      setGender("Kişi");
      setPhone("+994 (55) 432-10-98");
      setInsuranceType("İcbari Tibbi Sığorta");
      setAllergies("Penisillin (Dəridə səpgi)");
      setVitals({
        sys: 205,
        dia: 110,
        pulse: 132,
        spo2: 89,
        temp: 36.8,
        glucose: 7.4,
        vasPain: 9,
        gcs: 14,
      });
      setSelectedSymptoms(["chest_pain_crushing", "pain_radiating_arm_jaw", "cold_clammy_sweat"]);
      setClinicalNotes("Xəstə sol döş nahiyəsində kəskin sıxıcı ağrı və yapışqan tər ilə gətirildi.");
      setActiveSymptomCat("cardiovascular");
    } else if (preset === "STROKE") {
      setFullName("Zərifə Quliyeva");
      setFin("6B8X2W3");
      setBirthDate("1958-03-22");
      setGender("Qadın");
      setPhone("+994 (70) 987-65-43");
      setInsuranceType("Özəl Sığorta");
      setAllergies("Yod Kontrastına allergiya");
      setVitals({
        sys: 185,
        dia: 105,
        pulse: 90,
        spo2: 96,
        temp: 37.0,
        glucose: 6.2,
        vasPain: 6,
        gcs: 13,
      });
      setSelectedSymptoms(["facial_droop", "speech_difficulty", "unilateral_weakness"]);
      setClinicalNotes("FAST protokolu: üz əyilməsi və sağ hemiparez son 45 dəqiqədə başlayıb.");
      setActiveSymptomCat("neurology");
    } else if (preset === "PERITONITIS") {
      setFullName("Eldar Nəsirov");
      setFin("3R9K5P4");
      setBirthDate("1989-11-05");
      setGender("Kişi");
      setPhone("+994 (99) 555-44-33");
      setInsuranceType("İcbari Tibbi Sığorta");
      setAllergies("Yoxdur");
      setVitals({
        sys: 110,
        dia: 75,
        pulse: 118,
        spo2: 97,
        temp: 38.9,
        glucose: 5.5,
        vasPain: 9,
        gcs: 15,
      });
      setSelectedSymptoms(["board_rigid_abdomen", "severe_colic_pain"]);
      setClinicalNotes("Qarın divarında kəskin taxtavari gərginlik və Blumberq simptomu müsbətdir.");
      setActiveSymptomCat("abdominal");
    } else {
      setFullName("Aysel Məmmədli");
      setFin("1A2B3C4");
      setBirthDate("1996-09-18");
      setGender("Qadın");
      setPhone("+994 (50) 654-32-10");
      setInsuranceType("İcbari Tibbi Sığorta");
      setAllergies("Yoxdur");
      setVitals({
        sys: 118,
        dia: 78,
        pulse: 74,
        spo2: 99,
        temp: 36.6,
        glucose: 5.0,
        vasPain: 1,
        gcs: 15,
      });
      setSelectedSymptoms([]);
      setClinicalNotes("Planlı terapevtik müayinə üçün müraciət edib.");
      setActiveSymptomCat("cardiovascular");
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!fullName || !fin) {
      setStatusMessage({ type: "error", text: "Zəhmət olmasa Ad/Soyad və FİN kodu daxil edin!" });
      setActiveStep(1);
      return;
    }

    setSubmitting(true);
    setStatusMessage(null);

    try {
      const res = await fetch("/api/patients", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          fin,
          fullName,
          birthDate,
          gender,
          phone,
          insuranceType,
          vitals,
          symptoms: selectedSymptoms,
          clinicalNotes: `Allergiya: ${allergies}. ${clinicalNotes}`,
        }),
      });

      const json = await res.json();
      if (!res.ok || !json.success) {
        throw new Error(json.error || "Pasiyenti qeydiyyata almaq mümkün olmadı");
      }

      const { ticketNumber, cdss } = json.data;

      if (cdss.emergencyTrigger && cdss.alertType !== "NONE") {
        playEmergencyAlert(cdss.alertType);
        onEmergencyTriggered();
      }

      const printData: PrintableTicketData = {
        ticketNumber,
        fin,
        fullName,
        birthDate,
        gender,
        phone,
        insuranceType,
        registeredAt: new Date().toLocaleString("az-AZ"),
        triageLevel: cdss.triageLevel,
        triageColor: cdss.triageColor,
        triageWaitTime: cdss.triageWaitTime,
        assignedDepartment: cdss.assignedDepartment,
        assignedCabinet: cdss.assignedCabinet,
        assignedDoctor: cdss.assignedSpecialist,
        suspectedDiagnosis: cdss.suspectedDiagnosis,
        icd10Code: cdss.icd10Code,
        emergencyExams: cdss.emergencyExams,
        vitals,
        notes: `Allergiya: ${allergies}. ${clinicalNotes}`,
      };

      setStatusMessage({
        type: "success",
        text: `Pasiyent uğurla qeydiyyata alındı! Talon: ${ticketNumber} (${cdss.triageColor})`,
      });

      onPatientCreated(printData);
    } catch (err: any) {
      setStatusMessage({ type: "error", text: err.message });
    } finally {
      setSubmitting(false);
    }
  };

  const getTriageBadgeColor = (level: number) => {
    switch (level) {
      case 1:
        return "bg-rose-500/20 text-rose-300 border-rose-500/50 shadow-rose-950";
      case 2:
        return "bg-orange-500/20 text-orange-300 border-orange-500/50 shadow-orange-950";
      case 3:
        return "bg-amber-500/20 text-amber-300 border-amber-500/50 shadow-amber-950";
      case 4:
        return "bg-emerald-500/20 text-emerald-300 border-emerald-500/50 shadow-emerald-950";
      default:
        return "bg-sky-500/20 text-sky-300 border-sky-500/50 shadow-sky-950";
    }
  };

  const vAnalysis = cdssPreview.vitalsAnalysis;

  // Filter symptoms by active category
  const filteredSymptoms = SYMPTOM_DEFINITIONS.filter((s) => s.category === activeSymptomCat);

  return (
    <div className="space-y-4">
      {/* Top Bar: Presets & Ergonomic Mode Switcher */}
      <div className="flex flex-wrap items-center justify-between p-3 bg-slate-900/90 rounded-2xl border border-slate-800 gap-3">
        {/* Preset Buttons */}
        <div className="flex flex-wrap items-center gap-2">
          <span className="text-xs text-slate-400 font-semibold flex items-center gap-1.5 mr-1">
            <Sparkles className="w-3.5 h-3.5 text-cyan-400" />
            <span>Klinik Şablonlar:</span>
          </span>
          <button
            type="button"
            onClick={() => loadPreset("STEMI")}
            className="px-2.5 py-1 bg-rose-950/70 hover:bg-rose-900 text-rose-300 border border-rose-800/80 rounded-lg text-xs font-semibold transition"
          >
            ⚡ STEMI İnfarkt
          </button>
          <button
            type="button"
            onClick={() => loadPreset("STROKE")}
            className="px-2.5 py-1 bg-orange-950/70 hover:bg-orange-900 text-orange-300 border border-orange-800/80 rounded-lg text-xs font-semibold transition"
          >
            🚨 FAST+ İnsult
          </button>
          <button
            type="button"
            onClick={() => loadPreset("PERITONITIS")}
            className="px-2.5 py-1 bg-amber-950/70 hover:bg-amber-900 text-amber-300 border border-amber-800/80 rounded-lg text-xs font-semibold transition"
          >
            ⚠️ Cərrahi Qarın
          </button>
          <button
            type="button"
            onClick={() => loadPreset("NORMAL")}
            className="px-2.5 py-1 bg-slate-800 hover:bg-slate-700 text-slate-300 border border-slate-700 rounded-lg text-xs font-semibold transition"
          >
            🩺 Standart Qəbul
          </button>
        </div>

        {/* View Mode Toggle */}
        <div className="flex items-center gap-1 bg-slate-950 p-1 rounded-xl border border-slate-800 text-xs">
          <button
            type="button"
            onClick={() => setViewMode("stepped")}
            className={`flex items-center gap-1.5 px-3 py-1 rounded-lg font-bold transition ${
              viewMode === "stepped"
                ? "bg-cyan-600 text-white shadow-sm"
                : "text-slate-400 hover:text-slate-200"
            }`}
          >
            <SlidersHorizontal className="w-3.5 h-3.5" />
            <span>Addım-addım (Rahat Görünüş)</span>
          </button>
          <button
            type="button"
            onClick={() => setViewMode("all")}
            className={`flex items-center gap-1.5 px-3 py-1 rounded-lg font-bold transition ${
              viewMode === "all"
                ? "bg-cyan-600 text-white shadow-sm"
                : "text-slate-400 hover:text-slate-200"
            }`}
          >
            <LayoutGrid className="w-3.5 h-3.5" />
            <span>Geniş Panelli Görünüş</span>
          </button>
        </div>
      </div>

      {/* Main Guided Form Container */}
      <form onSubmit={handleSubmit} className="grid grid-cols-1 lg:grid-cols-12 gap-5">
        {/* LEFT / CENTER: WORK AREA (Cols 8) */}
        <div className="lg:col-span-8 space-y-4">
          {/* If Stepped View: Step Progress Bar */}
          {viewMode === "stepped" && (
            <div className="bg-slate-900/90 rounded-2xl p-2.5 border border-slate-800 flex items-center justify-between text-xs">
              <button
                type="button"
                onClick={() => setActiveStep(1)}
                className={`flex-1 flex items-center justify-center gap-2 py-2 px-3 rounded-xl font-bold transition ${
                  activeStep === 1
                    ? "bg-cyan-600 text-white shadow-md shadow-cyan-950"
                    : "text-slate-400 hover:text-white hover:bg-slate-800/60"
                }`}
              >
                <span className="w-5 h-5 rounded-full bg-slate-950/60 flex items-center justify-center text-[11px]">
                  1
                </span>
                <span>Pasiyent Rekvizitləri</span>
              </button>

              <ChevronRight className="w-4 h-4 text-slate-600 shrink-0 mx-1" />

              <button
                type="button"
                onClick={() => setActiveStep(2)}
                className={`flex-1 flex items-center justify-center gap-2 py-2 px-3 rounded-xl font-bold transition ${
                  activeStep === 2
                    ? "bg-cyan-600 text-white shadow-md shadow-cyan-950"
                    : "text-slate-400 hover:text-white hover:bg-slate-800/60"
                }`}
              >
                <span className="w-5 h-5 rounded-full bg-slate-950/60 flex items-center justify-center text-[11px]">
                  2
                </span>
                <span>Vital Göstəricilər</span>
              </button>

              <ChevronRight className="w-4 h-4 text-slate-600 shrink-0 mx-1" />

              <button
                type="button"
                onClick={() => setActiveStep(3)}
                className={`flex-1 flex items-center justify-center gap-2 py-2 px-3 rounded-xl font-bold transition ${
                  activeStep === 3
                    ? "bg-cyan-600 text-white shadow-md shadow-cyan-950"
                    : "text-slate-400 hover:text-white hover:bg-slate-800/60"
                }`}
              >
                <span className="w-5 h-5 rounded-full bg-slate-950/60 flex items-center justify-center text-[11px]">
                  3
                </span>
                <span>Şikayətlər ({selectedSymptoms.length})</span>
              </button>
            </div>
          )}

          {/* STEP 1: PATIENT REGISTRATION */}
          {(viewMode === "all" || activeStep === 1) && (
            <div className="bg-slate-900/90 rounded-2xl p-5 border border-slate-800 space-y-4 shadow-lg">
              <div className="flex items-center justify-between pb-3 border-b border-slate-800">
                <div className="flex items-center gap-2">
                  <User className="w-5 h-5 text-cyan-400" />
                  <h3 className="text-sm font-bold text-white uppercase tracking-wider">
                    Addım 1: Pasiyent Rekvizitləri və Qeydiyyat
                  </h3>
                </div>
                <span className="text-xs text-slate-400 font-mono">RECEPT-01</span>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-4 text-xs">
                <div>
                  <label className="text-slate-400 font-medium block mb-1">FİN Kod (7 simvol)*</label>
                  <input
                    type="text"
                    maxLength={7}
                    value={fin}
                    onChange={(e) => setFin(e.target.value.toUpperCase())}
                    placeholder="Məs: 7A8B9C1"
                    className="w-full bg-slate-950 border border-slate-800 rounded-xl px-3.5 py-2.5 text-white font-mono uppercase tracking-wider focus:border-cyan-500 focus:outline-none"
                    required
                  />
                </div>

                <div>
                  <label className="text-slate-400 font-medium block mb-1">Ad və Soyad*</label>
                  <input
                    type="text"
                    value={fullName}
                    onChange={(e) => setFullName(e.target.value)}
                    placeholder="Məs: Rəşad Əliyev"
                    className="w-full bg-slate-950 border border-slate-800 rounded-xl px-3.5 py-2.5 text-white font-semibold focus:border-cyan-500 focus:outline-none"
                    required
                  />
                </div>

                <div>
                  <div className="flex items-center justify-between mb-1">
                    <label className="text-slate-400 font-medium">Doğum Tarixi*</label>
                    <span className="text-[11px] text-cyan-400 font-mono font-bold">
                      {calculatedAge}
                    </span>
                  </div>
                  <input
                    type="date"
                    value={birthDate}
                    onChange={(e) => setBirthDate(e.target.value)}
                    className="w-full bg-slate-950 border border-slate-800 rounded-xl px-3 py-2 text-white focus:border-cyan-500 focus:outline-none"
                    required
                  />
                </div>

                <div>
                  <label className="text-slate-400 font-medium block mb-1">Cinsiyyət</label>
                  <select
                    value={gender}
                    onChange={(e) => setGender(e.target.value)}
                    className="w-full bg-slate-950 border border-slate-800 rounded-xl px-3 py-2.5 text-white focus:border-cyan-500 focus:outline-none"
                  >
                    <option value="Kişi">Kişi</option>
                    <option value="Qadın">Qadın</option>
                  </select>
                </div>

                <div>
                  <label className="text-slate-400 font-medium block mb-1">Əlaqə Telefonu*</label>
                  <input
                    type="text"
                    value={phone}
                    onChange={(e) => setPhone(e.target.value)}
                    placeholder="+994 (50) 000-00-00"
                    className="w-full bg-slate-950 border border-slate-800 rounded-xl px-3.5 py-2.5 text-white focus:border-cyan-500 focus:outline-none"
                    required
                  />
                </div>

                <div>
                  <label className="text-slate-400 font-medium block mb-1">Sığorta Təminatı</label>
                  <select
                    value={insuranceType}
                    onChange={(e) => setInsuranceType(e.target.value)}
                    className="w-full bg-slate-950 border border-slate-800 rounded-xl px-3 py-2.5 text-cyan-300 font-bold focus:border-cyan-500 focus:outline-none"
                  >
                    <option value="İcbari Tibbi Sığorta">İcbari Tibbi Sığorta (Dövlət İTS)</option>
                    <option value="Özəl Sığorta">Özəl Sığorta (Könüllü Korporativ)</option>
                    <option value="Nağd / Öz Hesabına">Nağd / Öz Hesabına</option>
                  </select>
                </div>
              </div>

              {/* Critical Drug Allergy Box */}
              <div className="pt-2 border-t border-slate-800/80">
                <label className="text-rose-400 font-bold block mb-1 flex items-center gap-1 text-xs">
                  <AlertCircle className="w-4 h-4" />
                  <span>Dərman Allergiyası (Klinik Qırmızı Bayraq)</span>
                </label>
                <input
                  type="text"
                  value={allergies}
                  onChange={(e) => setAllergies(e.target.value)}
                  placeholder="Məs: Penisillin, Yod kontrastı, Aspirin, Lateks..."
                  className="w-full bg-slate-950 border border-rose-900/60 rounded-xl px-3.5 py-2 text-rose-200 placeholder-slate-600 focus:border-rose-500 focus:outline-none text-xs"
                />
              </div>

              {viewMode === "stepped" && (
                <div className="flex justify-end pt-2">
                  <button
                    type="button"
                    onClick={() => setActiveStep(2)}
                    className="flex items-center gap-1.5 px-5 py-2.5 bg-cyan-600 hover:bg-cyan-500 text-white font-bold rounded-xl text-xs shadow-md transition"
                  >
                    <span>Növbəti: Vital Göstəricilər</span>
                    <ChevronRight className="w-4 h-4" />
                  </button>
                </div>
              )}
            </div>
          )}

          {/* STEP 2: VITALS & DIAGNOSTIC GAUGES */}
          {(viewMode === "all" || activeStep === 2) && (
            <div className="bg-slate-900/90 rounded-2xl p-5 border border-slate-800 space-y-4 shadow-lg">
              <div className="flex items-center justify-between pb-3 border-b border-slate-800">
                <div className="flex items-center gap-2">
                  <Activity className="w-5 h-5 text-rose-400" />
                  <h3 className="text-sm font-bold text-white uppercase tracking-wider">
                    Addım 2: Vital Göstəricilər və Şok İndeksi
                  </h3>
                </div>

                <div
                  className={`text-xs font-mono px-3 py-1 rounded-full font-bold flex items-center gap-1.5 border ${
                    vAnalysis.shockIndexStatus === "critical"
                      ? "bg-rose-500/20 text-rose-300 border-rose-500/50 animate-pulse"
                      : vAnalysis.shockIndexStatus === "elevated"
                      ? "bg-amber-500/20 text-amber-300 border-amber-500/50"
                      : "bg-emerald-500/20 text-emerald-300 border-emerald-500/50"
                  }`}
                  title={vAnalysis.shockIndexRemark}
                >
                  <Heart className="w-3.5 h-3.5" />
                  <span>Şok İndeksi: {vAnalysis.shockIndex}</span>
                </div>
              </div>

              <div className="grid grid-cols-2 md:grid-cols-3 gap-3 text-xs">
                {/* SYS / DIA */}
                <div className="bg-slate-950 p-3 rounded-xl border border-slate-800">
                  <div className="text-[11px] text-slate-400 font-medium flex items-center justify-between mb-1.5">
                    <span>Qan Təzyiqi</span>
                    <span className="text-slate-500 font-mono">mmHg</span>
                  </div>
                  <div className="flex items-center gap-1.5">
                    <input
                      type="number"
                      value={vitals.sys}
                      onChange={(e) => setVitals({ ...vitals, sys: parseInt(e.target.value) || 0 })}
                      className={`w-1/2 bg-slate-900 border rounded-lg px-2 py-1.5 font-mono font-bold text-center text-sm focus:outline-none ${
                        vAnalysis.sysInterpretation.status === "critical"
                          ? "border-rose-500 text-rose-400 animate-pulse"
                          : vAnalysis.sysInterpretation.status === "warning"
                          ? "border-amber-500 text-amber-300"
                          : "border-slate-700 text-white"
                      }`}
                    />
                    <span className="text-slate-600 font-bold text-base">/</span>
                    <input
                      type="number"
                      value={vitals.dia}
                      onChange={(e) => setVitals({ ...vitals, dia: parseInt(e.target.value) || 0 })}
                      className={`w-1/2 bg-slate-900 border rounded-lg px-2 py-1.5 font-mono font-bold text-center text-sm focus:outline-none ${
                        vAnalysis.diaInterpretation.status === "critical"
                          ? "border-rose-500 text-rose-400"
                          : "border-slate-700 text-white"
                      }`}
                    />
                  </div>
                  <div className="text-[10px] text-slate-400 mt-1.5 truncate">
                    {vAnalysis.sysInterpretation.clinicalRemark}
                  </div>
                </div>

                {/* Pulse */}
                <div className="bg-slate-950 p-3 rounded-xl border border-slate-800">
                  <div className="text-[11px] text-slate-400 font-medium flex items-center justify-between mb-1.5">
                    <span className="flex items-center gap-1">
                      <Heart className="w-3.5 h-3.5 text-rose-500" />
                      <span>Nəbz (Ürək ritmi)</span>
                    </span>
                    <span className="text-slate-500 font-mono">BPM</span>
                  </div>
                  <input
                    type="number"
                    value={vitals.pulse}
                    onChange={(e) => setVitals({ ...vitals, pulse: parseInt(e.target.value) || 0 })}
                    className={`w-full bg-slate-900 border rounded-lg px-3 py-1.5 font-mono font-bold text-center text-sm focus:outline-none ${
                      vAnalysis.pulseInterpretation.status === "critical"
                        ? "border-rose-500 text-rose-400"
                        : "border-slate-700 text-white"
                    }`}
                  />
                  <div className="text-[10px] text-slate-400 mt-1.5 truncate">
                    {vAnalysis.pulseInterpretation.clinicalRemark}
                  </div>
                </div>

                {/* SpO2 */}
                <div className="bg-slate-950 p-3 rounded-xl border border-slate-800">
                  <div className="text-[11px] text-slate-400 font-medium flex items-center justify-between mb-1.5">
                    <span className="flex items-center gap-1">
                      <Wind className="w-3.5 h-3.5 text-cyan-400" />
                      <span>SpO2 Saturasiya</span>
                    </span>
                    <span className="text-slate-500 font-mono">%</span>
                  </div>
                  <input
                    type="number"
                    value={vitals.spo2}
                    onChange={(e) => setVitals({ ...vitals, spo2: parseInt(e.target.value) || 0 })}
                    className={`w-full bg-slate-900 border rounded-lg px-3 py-1.5 font-mono font-bold text-center text-sm focus:outline-none ${
                      vAnalysis.spo2Interpretation.status === "critical"
                        ? "border-rose-500 text-rose-400 animate-pulse"
                        : vAnalysis.spo2Interpretation.status === "warning"
                        ? "border-amber-500 text-amber-300"
                        : "border-slate-700 text-white"
                    }`}
                  />
                  <div className="text-[10px] text-slate-400 mt-1.5 truncate">
                    {vAnalysis.spo2Interpretation.clinicalRemark}
                  </div>
                </div>

                {/* Temp */}
                <div className="bg-slate-950 p-3 rounded-xl border border-slate-800">
                  <div className="text-[11px] text-slate-400 font-medium flex items-center justify-between mb-1.5">
                    <span className="flex items-center gap-1">
                      <Thermometer className="w-3.5 h-3.5 text-amber-400" />
                      <span>Bədən Hərarəti</span>
                    </span>
                    <span className="text-slate-500 font-mono">°C</span>
                  </div>
                  <input
                    type="number"
                    step="0.1"
                    value={vitals.temp}
                    onChange={(e) => setVitals({ ...vitals, temp: parseFloat(e.target.value) || 0 })}
                    className={`w-full bg-slate-900 border rounded-lg px-3 py-1.5 font-mono font-bold text-center text-sm focus:outline-none ${
                      vAnalysis.tempInterpretation.status === "critical"
                        ? "border-rose-500 text-rose-400"
                        : "border-slate-700 text-white"
                    }`}
                  />
                  <div className="text-[10px] text-slate-400 mt-1.5 truncate">
                    {vAnalysis.tempInterpretation.clinicalRemark}
                  </div>
                </div>

                {/* Glucose */}
                <div className="bg-slate-950 p-3 rounded-xl border border-slate-800">
                  <div className="text-[11px] text-slate-400 font-medium flex items-center justify-between mb-1.5">
                    <span className="flex items-center gap-1">
                      <Droplet className="w-3.5 h-3.5 text-blue-400" />
                      <span>Qlükoza</span>
                    </span>
                    <span className="text-slate-500 font-mono">mmol/L</span>
                  </div>
                  <input
                    type="number"
                    step="0.1"
                    value={vitals.glucose}
                    onChange={(e) => setVitals({ ...vitals, glucose: parseFloat(e.target.value) || 0 })}
                    className={`w-full bg-slate-900 border rounded-lg px-3 py-1.5 font-mono font-bold text-center text-sm focus:outline-none ${
                      vAnalysis.glucoseInterpretation.status === "critical"
                        ? "border-rose-500 text-rose-400"
                        : "border-slate-700 text-white"
                    }`}
                  />
                  <div className="text-[10px] text-slate-400 mt-1.5 truncate">
                    {vAnalysis.glucoseInterpretation.clinicalRemark}
                  </div>
                </div>

                {/* VAS Pain */}
                <div className="bg-slate-950 p-3 rounded-xl border border-slate-800">
                  <div className="text-[11px] text-slate-400 font-medium flex items-center justify-between mb-1.5">
                    <span className="flex items-center gap-1">
                      <Zap className="w-3.5 h-3.5 text-yellow-400" />
                      <span>Ağrı Şkalası (VAS)</span>
                    </span>
                    <span
                      className={`font-mono font-bold text-xs ${
                        vitals.vasPain >= 8
                          ? "text-rose-400 font-black"
                          : vitals.vasPain >= 4
                          ? "text-amber-300"
                          : "text-emerald-400"
                      }`}
                    >
                      {vitals.vasPain}/10
                    </span>
                  </div>
                  <input
                    type="range"
                    min="0"
                    max="10"
                    value={vitals.vasPain}
                    onChange={(e) => setVitals({ ...vitals, vasPain: parseInt(e.target.value) })}
                    className="w-full h-2 bg-slate-800 rounded-lg appearance-none cursor-pointer accent-cyan-500 mt-2"
                  />
                  <div className="text-[10px] text-slate-400 mt-1.5 truncate">
                    {vAnalysis.vasInterpretation.clinicalRemark}
                  </div>
                </div>
              </div>

              {viewMode === "stepped" && (
                <div className="flex justify-between pt-2">
                  <button
                    type="button"
                    onClick={() => setActiveStep(1)}
                    className="flex items-center gap-1.5 px-4 py-2 bg-slate-800 hover:bg-slate-700 text-slate-300 rounded-xl text-xs font-semibold transition"
                  >
                    <ChevronLeft className="w-4 h-4" />
                    <span>Əvvəlki: Pasiyent</span>
                  </button>
                  <button
                    type="button"
                    onClick={() => setActiveStep(3)}
                    className="flex items-center gap-1.5 px-5 py-2.5 bg-cyan-600 hover:bg-cyan-500 text-white font-bold rounded-xl text-xs shadow-md transition"
                  >
                    <span>Növbəti: Şikayətlər</span>
                    <ChevronRight className="w-4 h-4" />
                  </button>
                </div>
              )}
            </div>
          )}

          {/* STEP 3: CATEGORIZED SYMPTOMS CHECKLIST */}
          {(viewMode === "all" || activeStep === 3) && (
            <div className="bg-slate-900/90 rounded-2xl p-5 border border-slate-800 space-y-4 shadow-lg">
              <div className="flex items-center justify-between pb-3 border-b border-slate-800">
                <div className="flex items-center gap-2">
                  <CheckSquare className="w-5 h-5 text-cyan-400" />
                  <h3 className="text-sm font-bold text-white uppercase tracking-wider">
                    Addım 3: Klinik Simptomlar Çeklisti
                  </h3>
                </div>
                <div className="flex items-center gap-2">
                  <span className="text-xs bg-slate-800 text-cyan-300 font-mono px-2 py-0.5 rounded font-bold">
                    Seçilib: {selectedSymptoms.length}
                  </span>
                  {selectedSymptoms.length > 0 && (
                    <button
                      type="button"
                      onClick={() => setSelectedSymptoms([])}
                      className="text-xs text-rose-400 hover:underline"
                    >
                      Təmizlə
                    </button>
                  )}
                </div>
              </div>

              {/* Categorized Symptom Tabs */}
              <div className="flex flex-wrap gap-1.5 p-1 bg-slate-950 rounded-xl border border-slate-800 text-xs">
                <button
                  type="button"
                  onClick={() => setActiveSymptomCat("cardiovascular")}
                  className={`px-3 py-1.5 rounded-lg font-bold transition ${
                    activeSymptomCat === "cardiovascular"
                      ? "bg-rose-950 text-rose-200 border border-rose-800"
                      : "text-slate-400 hover:text-white"
                  }`}
                >
                  Ürək-damar
                </button>
                <button
                  type="button"
                  onClick={() => setActiveSymptomCat("respiratory")}
                  className={`px-3 py-1.5 rounded-lg font-bold transition ${
                    activeSymptomCat === "respiratory"
                      ? "bg-cyan-950 text-cyan-200 border border-cyan-800"
                      : "text-slate-400 hover:text-white"
                  }`}
                >
                  Tənəffüs
                </button>
                <button
                  type="button"
                  onClick={() => setActiveSymptomCat("neurology")}
                  className={`px-3 py-1.5 rounded-lg font-bold transition ${
                    activeSymptomCat === "neurology"
                      ? "bg-orange-950 text-orange-200 border border-orange-800"
                      : "text-slate-400 hover:text-white"
                  }`}
                >
                  Nevrologiya (FAST)
                </button>
                <button
                  type="button"
                  onClick={() => setActiveSymptomCat("abdominal")}
                  className={`px-3 py-1.5 rounded-lg font-bold transition ${
                    activeSymptomCat === "abdominal"
                      ? "bg-amber-950 text-amber-200 border border-amber-800"
                      : "text-slate-400 hover:text-white"
                  }`}
                >
                  Qarın Boşluğu
                </button>
                <button
                  type="button"
                  onClick={() => setActiveSymptomCat("trauma_shock")}
                  className={`px-3 py-1.5 rounded-lg font-bold transition ${
                    activeSymptomCat === "trauma_shock"
                      ? "bg-red-950 text-red-200 border border-red-800"
                      : "text-slate-400 hover:text-white"
                  }`}
                >
                  Travma & Şok
                </button>
              </div>

              {/* Symptom Cards Grid for Active Category */}
              <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
                {filteredSymptoms.map((symptom) => {
                  const isChecked = selectedSymptoms.includes(symptom.id);
                  return (
                    <button
                      type="button"
                      key={symptom.id}
                      onClick={() => toggleSymptom(symptom.id)}
                      className={`p-3 rounded-xl border text-left flex items-start gap-3 transition ${
                        isChecked
                          ? "bg-cyan-950/50 border-cyan-500 shadow-md shadow-cyan-950 text-cyan-200"
                          : "bg-slate-950 border-slate-800/80 text-slate-300 hover:bg-slate-800/40"
                      }`}
                    >
                      <div className="mt-0.5 shrink-0">
                        {isChecked ? (
                          <CheckSquare className="w-4 h-4 text-cyan-400" />
                        ) : (
                          <Square className="w-4 h-4 text-slate-600" />
                        )}
                      </div>
                      <div>
                        <div className="text-xs font-bold leading-tight">{symptom.label}</div>
                        <div className="text-[11px] text-slate-400 mt-1 leading-snug">
                          {symptom.description}
                        </div>
                      </div>
                    </button>
                  );
                })}
              </div>

              {viewMode === "stepped" && (
                <div className="flex justify-start pt-2">
                  <button
                    type="button"
                    onClick={() => setActiveStep(2)}
                    className="flex items-center gap-1.5 px-4 py-2 bg-slate-800 hover:bg-slate-700 text-slate-300 rounded-xl text-xs font-semibold transition"
                  >
                    <ChevronLeft className="w-4 h-4" />
                    <span>Əvvəlki: Vital Göstəricilər</span>
                  </button>
                </div>
              )}
            </div>
          )}
        </div>

        {/* RIGHT COLUMN: REAL-TIME CDSS DECISION CARD (Cols 4) */}
        <div className="lg:col-span-4 bg-slate-900/90 rounded-2xl p-5 border border-slate-800 flex flex-col justify-between space-y-4 shadow-xl sticky top-24 self-start">
          <div>
            <div className="flex items-center justify-between pb-3 border-b border-slate-800">
              <div className="flex items-center gap-2">
                <Stethoscope className="w-5 h-5 text-cyan-400" />
                <h3 className="text-sm font-bold text-white uppercase tracking-wider">
                  CDSS Ekspert Qərarı
                </h3>
              </div>
              <span className="text-[10px] font-mono text-cyan-400 bg-cyan-950 px-2 py-0.5 rounded border border-cyan-800 font-bold">
                MTS ENGINE
              </span>
            </div>

            {/* Manchester Triage Badge */}
            <div className="mt-4">
              <div
                className={`p-4 rounded-2xl border shadow-xl ${getTriageBadgeColor(
                  cdssPreview.triageLevel
                )}`}
              >
                <div className="flex items-center justify-between text-xs">
                  <span className="font-bold uppercase tracking-wider">
                    Manchester Prioriteti (MTS)
                  </span>
                  <span className="font-mono font-bold bg-slate-950/60 px-2 py-0.5 rounded">
                    Maks: {cdssPreview.triageWaitTime}
                  </span>
                </div>
                <div className="text-2xl font-black mt-2">
                  SƏVİYYƏ {cdssPreview.triageLevel}: {cdssPreview.triageColor}
                </div>
                <p className="text-xs mt-2 opacity-90 leading-relaxed font-medium">
                  {cdssPreview.clinicalSummary}
                </p>
              </div>
            </div>

            {/* Suspected Diagnosis Box */}
            <div className="mt-4 bg-slate-950 p-4 rounded-xl border border-slate-800 space-y-2 text-xs">
              <div>
                <span className="text-slate-400 block text-[10px] uppercase font-bold tracking-wider">
                  İlkin Diaqnoz Şübhəsi:
                </span>
                <span className="text-white font-bold text-sm block mt-1 leading-snug">
                  {cdssPreview.suspectedDiagnosis}
                </span>
                <span className="inline-block mt-1.5 text-[10px] font-mono bg-slate-900 px-2 py-0.5 rounded text-cyan-400 border border-slate-800 font-bold">
                  XBT-10 / ICD-10: {cdssPreview.icd10Code}
                </span>
              </div>

              <div className="border-t border-slate-800 pt-2.5 flex items-center justify-between">
                <div>
                  <span className="text-slate-400 block text-[10px]">Təyin Edilən Şöbə:</span>
                  <span className="text-cyan-300 font-bold text-xs">{cdssPreview.assignedDepartment}</span>
                </div>
                <div className="text-right">
                  <span className="text-slate-400 block text-[10px]">Kabinet:</span>
                  <span className="text-white font-mono font-bold bg-slate-800 px-2 py-0.5 rounded text-xs">
                    {cdssPreview.assignedCabinet}
                  </span>
                </div>
              </div>
            </div>

            {/* STAT Emergency Exams */}
            <div className="mt-4 bg-slate-950 p-3.5 rounded-xl border border-slate-800 text-xs">
              <span className="text-slate-400 block text-[10px] uppercase font-bold tracking-wider mb-2">
                Təcili Tələb Olunan STAT Müayinələr:
              </span>
              <ul className="space-y-1.5">
                {cdssPreview.emergencyExams.map((exam, i) => (
                  <li key={i} className="text-slate-300 text-[11px] flex items-center gap-2">
                    <span className="w-1.5 h-1.5 rounded-full bg-cyan-400 shrink-0" />
                    <span>{exam}</span>
                  </li>
                ))}
              </ul>
            </div>
          </div>

          {/* Action & Registration Button */}
          <div className="space-y-2.5 pt-3 border-t border-slate-800">
            {statusMessage && (
              <div
                className={`p-3 rounded-xl text-xs font-medium flex items-center gap-2 ${
                  statusMessage.type === "success"
                    ? "bg-emerald-950/80 text-emerald-300 border border-emerald-500/40"
                    : "bg-rose-950/80 text-rose-300 border border-rose-500/40"
                }`}
              >
                <CheckCircle2 className="w-4 h-4 shrink-0" />
                <span>{statusMessage.text}</span>
              </div>
            )}

            <button
              type="submit"
              disabled={submitting}
              className="w-full py-4 px-4 bg-gradient-to-r from-cyan-600 via-blue-600 to-indigo-600 hover:from-cyan-500 hover:to-indigo-500 text-white font-black rounded-xl text-sm shadow-xl shadow-cyan-950 transition flex items-center justify-center gap-2 disabled:opacity-50"
            >
              <Send className="w-4 h-4" />
              <span>{submitting ? "Qeydiyyata Alınır..." : "Qeydiyyata Al və Rəsmi Talon Yarat"}</span>
            </button>
          </div>
        </div>
      </form>
    </div>
  );
}
