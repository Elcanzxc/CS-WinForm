"use client";

import React, { useState, useEffect } from "react";
import {
  AlertOctagon,
  BellRing,
  CheckCircle2,
  PhoneCall,
  UserCheck,
  VolumeX,
  X,
  Radio,
  Flame,
  Check,
  Clock,
  RadioTower,
  Stethoscope,
  RefreshCw,
  RotateCcw,
  Send,
  Building,
} from "lucide-react";
import { stopCurrentAlarm, playHospitalCallChime } from "@/lib/audioAlerts";

export interface EmergencyAlertItem {
  id: number;
  code_type: "CODE_BLUE" | "CODE_RED" | "CODE_YELLOW";
  ticket_number?: string;
  patient_name?: string;
  location: string;
  reason: string;
  status: string;
  ambulance_called?: number;
  doctor_notified?: number;
  created_at: string;
}

export interface DoctorOption {
  cabinet_no: string;
  department: string;
  doctor_name: string;
  doctor_specialty: string;
  phone_ext: string;
  status: string;
}

export interface DispatchItem {
  id: number;
  alert_id: number;
  dispatch_type: string;
  target_team: string;
  channel: string;
  status: string;
  message: string;
  created_at: string;
}

interface EmergencyOverlayProps {
  alerts: EmergencyAlertItem[];
  onAcknowledge: (alertId: number) => Promise<void>;
  onRefreshNeeded?: () => void;
}

const BRIGADE_OPTIONS = [
  "Reanimasiya və Şok Zalı Briqadası #1 (Kanal 1 Radio)",
  "Kardioreanimasiya və STEMI Briqadası #2 (Kanal 2 Radio)",
  "İnsult və Nevrologiya Mobil Briqadası #3 (Kanal 3 Radio)",
  "Təcili Politravma və Cərrahiyyə Briqadası #4 (Kanal 4 Radio)",
];

export default function EmergencyOverlay({
  alerts,
  onAcknowledge,
  onRefreshNeeded,
}: EmergencyOverlayProps) {
  const [minimized, setMinimized] = useState(false);
  const [doctors, setDoctors] = useState<DoctorOption[]>([]);
  const [selectedCabinetNo, setSelectedCabinetNo] = useState<string>("");
  const [selectedBrigade, setSelectedBrigade] = useState<string>(BRIGADE_OPTIONS[0]);

  const [dispatches, setDispatches] = useState<DispatchItem[]>([]);
  const [callingAmbulance, setCallingAmbulance] = useState(false);
  const [notifyingDoctor, setNotifyingDoctor] = useState(false);
  const [resettingDispatches, setResettingDispatches] = useState(false);
  const [ambulanceDispatched, setAmbulanceDispatched] = useState(false);
  const [doctorNotified, setDoctorNotified] = useState(false);
  const [lastDeliveredMsg, setLastDeliveredMsg] = useState<string | null>(null);

  const activeAlerts = alerts.filter((a) => a.status === "ACTIVE");
  const currentAlert = activeAlerts.length > 0 ? activeAlerts[0] : null;

  // Load doctors & dispatches specifically scoped to the active alert
  useEffect(() => {
    if (!currentAlert) {
      setDispatches([]);
      setAmbulanceDispatched(false);
      setDoctorNotified(false);
      setLastDeliveredMsg(null);
      return;
    }

    let isMounted = true;

    const loadAlertContext = async () => {
      try {
        const res = await fetch(`/api/alerts?alertId=${currentAlert.id}`);
        const json = await res.json();
        if (json.success && isMounted) {
          const docList: DoctorOption[] = json.doctors || [];
          setDoctors(docList);
          setDispatches(json.recentDispatches || []);
          setAmbulanceDispatched(currentAlert.ambulance_called === 1);
          setDoctorNotified(currentAlert.doctor_notified === 1);
          setLastDeliveredMsg(null);

          // Intelligently pick default on-duty doctor based on reason/location
          const reasonLower = (currentAlert.reason + " " + currentAlert.location).toLowerCase();
          let matchedCabinet = "";

          if (reasonLower.includes("insult") || reasonLower.includes("fast") || reasonLower.includes("neyro")) {
            matchedCabinet = docList.find((d) => d.cabinet_no.startsWith("KAB-201") || d.department.toLowerCase().includes("nevro"))?.cabinet_no || "";
          } else if (reasonLower.includes("kardio") || reasonLower.includes("stemi") || reasonLower.includes("infarkt") || reasonLower.includes("ürək")) {
            matchedCabinet = docList.find((d) => d.cabinet_no.startsWith("KAB-101") || d.department.toLowerCase().includes("kardio"))?.cabinet_no || "";
          } else if (reasonLower.includes("cərrah") || reasonLower.includes("apandisit") || reasonLower.includes("qarın")) {
            matchedCabinet = docList.find((d) => d.cabinet_no.startsWith("CƏR") || d.department.toLowerCase().includes("cərrah"))?.cabinet_no || "";
          } else if (reasonLower.includes("travma") || reasonLower.includes("sınıq")) {
            matchedCabinet = docList.find((d) => d.cabinet_no.startsWith("TRAV") || d.department.toLowerCase().includes("travma"))?.cabinet_no || "";
          }

          if (!matchedCabinet && docList.length > 0) {
            // Default to ICU / Resuscitation doctor (REA-01) or first available
            const reaDoctor = docList.find((d) => d.cabinet_no.startsWith("REA-01"));
            matchedCabinet = reaDoctor ? reaDoctor.cabinet_no : docList[0].cabinet_no;
          }

          if (matchedCabinet) {
            setSelectedCabinetNo(matchedCabinet);
          }
        }
      } catch (err) {
        console.error("Error loading alert context:", err);
      }
    };

    loadAlertContext();

    return () => {
      isMounted = false;
    };
  }, [currentAlert?.id, currentAlert?.ambulance_called, currentAlert?.doctor_notified]);

  if (!currentAlert) return null;

  const isBlue = currentAlert.code_type === "CODE_BLUE";
  const isRed = currentAlert.code_type === "CODE_RED";

  const selectedDoctor = doctors.find((d) => d.cabinet_no === selectedCabinetNo);

  const handleAcknowledge = async () => {
    stopCurrentAlarm();
    await onAcknowledge(currentAlert.id);
    // Reset state cleanly for future alerts
    setDispatches([]);
    setAmbulanceDispatched(false);
    setDoctorNotified(false);
    setLastDeliveredMsg(null);
  };

  const handleCallAmbulance = async () => {
    try {
      setCallingAmbulance(true);
      const res = await fetch("/api/alerts", {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          alertId: currentAlert.id,
          action: "CALL_AMBULANCE",
          ambulanceTeam: selectedBrigade,
          cabinetNo: selectedCabinetNo || "REA-01",
        }),
      });
      const json = await res.json();
      if (json.success) {
        playHospitalCallChime();
        setAmbulanceDispatched(true);
        setLastDeliveredMsg(json.message);

        // Fetch fresh dispatches for this alert
        const refreshRes = await fetch(`/api/alerts?alertId=${currentAlert.id}`);
        const refreshJson = await refreshRes.json();
        if (refreshJson.success) {
          setDispatches(refreshJson.recentDispatches || []);
        }

        if (onRefreshNeeded) onRefreshNeeded();
      }
    } catch (err) {
      console.error(err);
    } finally {
      setCallingAmbulance(false);
    }
  };

  const handleNotifyDoctor = async () => {
    if (!selectedDoctor) return;
    try {
      setNotifyingDoctor(true);
      const res = await fetch("/api/alerts", {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          alertId: currentAlert.id,
          action: "NOTIFY_DOCTOR",
          doctorName: selectedDoctor.doctor_name,
          doctorSpecialty: selectedDoctor.doctor_specialty,
          cabinetNo: selectedDoctor.cabinet_no,
        }),
      });
      const json = await res.json();
      if (json.success) {
        playHospitalCallChime();
        setDoctorNotified(true);
        setLastDeliveredMsg(json.message);

        // Fetch fresh dispatches for this alert
        const refreshRes = await fetch(`/api/alerts?alertId=${currentAlert.id}`);
        const refreshJson = await refreshRes.json();
        if (refreshJson.success) {
          setDispatches(refreshJson.recentDispatches || []);
        }

        if (onRefreshNeeded) onRefreshNeeded();
      }
    } catch (err) {
      console.error(err);
    } finally {
      setNotifyingDoctor(false);
    }
  };

  const handleResetDispatches = async () => {
    try {
      setResettingDispatches(true);
      const res = await fetch("/api/alerts", {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          alertId: currentAlert.id,
          action: "RESET_DISPATCHES",
        }),
      });
      const json = await res.json();
      if (json.success) {
        setDispatches([]);
        setAmbulanceDispatched(false);
        setDoctorNotified(false);
        setLastDeliveredMsg(null);
        if (onRefreshNeeded) onRefreshNeeded();
      }
    } catch (err) {
      console.error(err);
    } finally {
      setResettingDispatches(false);
    }
  };

  const formatTimeSafe = (timeStr?: string) => {
    if (!timeStr) return "";
    try {
      const d = new Date(timeStr);
      return isNaN(d.getTime()) ? timeStr : d.toLocaleTimeString("az-AZ");
    } catch {
      return timeStr;
    }
  };

  return (
    <>
      {/* Perimeter Animated Screen Border Glow */}
      <div
        className={`fixed inset-0 pointer-events-none z-50 border-[6px] ${
          isBlue ? "animate-blue-alarm border-blue-500" : "animate-perimeter-alarm border-rose-600"
        }`}
      />

      {/* Floating Modal / Alert Card */}
      {!minimized ? (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/85 backdrop-blur-md">
          <div
            className={`w-full max-w-2xl rounded-2xl p-5 sm:p-6 shadow-2xl border transition-all ${
              isBlue
                ? "bg-slate-950/95 border-blue-500 shadow-blue-900/50"
                : "bg-slate-950/95 border-rose-500 shadow-rose-900/50"
            }`}
          >
            {/* Header Badge */}
            <div className="flex items-center justify-between pb-3.5 border-b border-slate-800">
              <div className="flex items-center space-x-3">
                <div
                  className={`p-3 rounded-xl shadow-md ${
                    isBlue ? "bg-blue-600 text-white animate-pulse" : "bg-rose-600 text-white animate-bounce"
                  }`}
                >
                  {isBlue ? <Radio className="w-7 h-7" /> : <Flame className="w-7 h-7" />}
                </div>
                <div>
                  <div className="flex items-center gap-2">
                    <span
                      className={`text-[10px] sm:text-xs font-black px-2.5 py-0.5 rounded-full uppercase tracking-wider text-white ${
                        isBlue ? "bg-blue-600" : "bg-rose-600"
                      }`}
                    >
                      {currentAlert.code_type === "CODE_BLUE"
                        ? "CODE BLUE • REANİMASİYA"
                        : currentAlert.code_type === "CODE_RED"
                        ? "CODE RED • TƏCİLİ KRİTİK ŞOK"
                        : "CODE YELLOW • DİQQƏT"}
                    </span>
                    <span className="text-xs text-slate-400 font-mono">
                      #{currentAlert.id} • {formatTimeSafe(currentAlert.created_at)}
                    </span>
                  </div>
                  <h2 className="text-lg sm:text-xl font-black text-white mt-1">
                    TƏCİLİ KLİNİK MÜDAXİLƏ TƏLƏB OLUNUR!
                  </h2>
                </div>
              </div>

              <button
                onClick={() => setMinimized(true)}
                title="Pəncərəni yığışdır"
                className="text-slate-400 hover:text-white p-1 rounded-lg hover:bg-slate-800 transition"
              >
                <X className="w-5 h-5" />
              </button>
            </div>

            {/* Alert Core Details */}
            <div className="my-3 space-y-2 bg-slate-900/80 p-3.5 rounded-xl border border-slate-800 text-xs sm:text-sm">
              <div className="flex justify-between items-center border-b border-slate-800/80 pb-1.5">
                <span className="text-slate-400">Pasiyent / Talon:</span>
                <span className="text-white font-bold font-mono">
                  {currentAlert.patient_name || "Məlum deyil"}{" "}
                  {currentAlert.ticket_number ? `(${currentAlert.ticket_number})` : ""}
                </span>
              </div>
              <div className="flex justify-between items-center border-b border-slate-800/80 pb-1.5">
                <span className="text-slate-400">Məkan / Şöbə:</span>
                <span className="text-cyan-400 font-semibold">{currentAlert.location}</span>
              </div>
              <div className="pt-0.5">
                <span className="text-slate-400 block mb-1 text-xs">Təcili Vəziyyət Səbəbi:</span>
                <p className="text-rose-300 font-medium bg-rose-950/40 p-2 rounded-lg border border-rose-900/60 leading-relaxed text-xs">
                  {currentAlert.reason}
                </p>
              </div>
            </div>

            {/* CLINICAL DISPATCH ROUTING CONTROLS (DOCTOR & BRIGADE SELECTOR) */}
            <div className="my-3 p-3.5 bg-slate-900/90 rounded-xl border border-cyan-500/30 space-y-3">
              <div className="flex items-center justify-between text-xs pb-1 border-b border-slate-800">
                <span className="font-bold text-cyan-400 flex items-center gap-1.5 uppercase tracking-wider text-[11px]">
                  <Stethoscope className="w-3.5 h-3.5" />
                  <span>Klinik Çağırış və Yönləndirmə Marşrutu</span>
                </span>
                <span className="text-[10px] text-slate-400 font-mono">
                  Baza: {doctors.length} Növbətçi Mütəxəssis
                </span>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-2.5 text-xs">
                {/* 1. Target Doctor Selector */}
                <div>
                  <label className="text-slate-300 block mb-1 font-semibold flex items-center gap-1">
                    <UserCheck className="w-3.5 h-3.5 text-cyan-400" />
                    <span>Növbətçi Həkim (GSM / Pager):</span>
                  </label>
                  <select
                    value={selectedCabinetNo}
                    onChange={(e) => setSelectedCabinetNo(e.target.value)}
                    className="w-full bg-slate-950 border border-slate-700 text-white rounded-lg px-2.5 py-1.5 text-xs font-medium focus:border-cyan-500 focus:outline-none"
                  >
                    {doctors.map((doc) => (
                      <option key={doc.cabinet_no} value={doc.cabinet_no}>
                        {doc.doctor_name} — {doc.doctor_specialty} ({doc.cabinet_no})
                      </option>
                    ))}
                  </select>
                  {selectedDoctor && (
                    <div className="mt-1 flex items-center justify-between text-[10px] text-slate-400">
                      <span>Şöbə: {selectedDoctor.department}</span>
                      <span
                        className={`font-mono px-1.5 py-0.2 rounded ${
                          selectedDoctor.status === "Təcili çağırış"
                            ? "bg-rose-950 text-rose-300 border border-rose-800"
                            : selectedDoctor.status === "Boşdur"
                            ? "bg-emerald-950 text-emerald-300 border border-emerald-800"
                            : "bg-slate-800 text-slate-300"
                        }`}
                      >
                        {selectedDoctor.status} (Ext: {selectedDoctor.phone_ext})
                      </span>
                    </div>
                  )}
                </div>

                {/* 2. Target Brigade Selector */}
                <div>
                  <label className="text-slate-300 block mb-1 font-semibold flex items-center gap-1">
                    <RadioTower className="w-3.5 h-3.5 text-rose-400" />
                    <span>Reanimasiya Briqadası (Radio Şəbəkə):</span>
                  </label>
                  <select
                    value={selectedBrigade}
                    onChange={(e) => setSelectedBrigade(e.target.value)}
                    className="w-full bg-slate-950 border border-slate-700 text-white rounded-lg px-2.5 py-1.5 text-xs font-medium focus:border-rose-500 focus:outline-none"
                  >
                    {BRIGADE_OPTIONS.map((brigade) => (
                      <option key={brigade} value={brigade}>
                        {brigade}
                      </option>
                    ))}
                  </select>
                  <div className="mt-1 text-[10px] text-slate-400">
                    Kanal: Şifrəli Təcili Xəstəxana Tezliyi (STAT Dispatch)
                  </div>
                </div>
              </div>
            </div>

            {/* Live Operational Action Receipts (SCOPED TO THIS ALERT) */}
            <div className="mb-3 p-3 bg-slate-900 rounded-xl border border-emerald-500/40 space-y-1.5 text-xs max-h-[140px] overflow-y-auto">
              <div className="flex items-center justify-between text-[10px] uppercase font-bold text-emerald-400 tracking-wider">
                <span className="flex items-center gap-1.5">
                  <CheckCircle2 className="w-3.5 h-3.5" />
                  <span>Bu Həyəcan Üzrə Qeydə Alınan Çatdırılmalar:</span>
                </span>
                {dispatches.length > 0 && (
                  <button
                    onClick={handleResetDispatches}
                    disabled={resettingDispatches}
                    className="flex items-center gap-1 text-[10px] text-slate-400 hover:text-rose-400 transition"
                    title="Bu həyəcan üzrə rabitə qeydlərini sıfırla"
                  >
                    <RotateCcw className="w-3 h-3" />
                    <span>Sıfırla</span>
                  </button>
                )}
              </div>

              {dispatches.length > 0 ? (
                dispatches.map((log) => (
                  <div
                    key={log.id}
                    className="flex items-start justify-between gap-2 text-slate-200 bg-slate-950/70 p-1.5 rounded-lg border border-slate-800 text-[11px]"
                  >
                    <div className="space-y-0.5">
                      <div className="flex items-center gap-1.5">
                        <span
                          className={`text-[9px] font-bold px-1 py-0.2 rounded uppercase ${
                            log.dispatch_type === "DOCTOR_PAGER"
                              ? "bg-cyan-950 text-cyan-300 border border-cyan-800"
                              : "bg-rose-950 text-rose-300 border border-rose-800"
                          }`}
                        >
                          {log.dispatch_type === "DOCTOR_PAGER" ? "HƏKİM PAGER" : "BRİQADA"}
                        </span>
                        <span className="font-semibold text-white">{log.target_team}</span>
                      </div>
                      <div className="text-[10px] text-slate-400">
                        {log.message} • Kanal: {log.channel}
                      </div>
                    </div>
                    <div className="text-right shrink-0">
                      <span className="text-[9px] font-bold text-emerald-400 bg-emerald-950/80 px-1 py-0.5 rounded border border-emerald-800 block">
                        {log.status}
                      </span>
                      <span className="text-[10px] font-mono text-cyan-300 font-bold block mt-0.5">
                        {formatTimeSafe(log.created_at)}
                      </span>
                    </div>
                  </div>
                ))
              ) : (
                <div className="text-[11px] text-slate-400 italic py-1 text-center bg-slate-950/40 rounded-lg">
                  Bu həyəcan üzrə hələ bildiriş göndərilməyib. Yuxarıdakı həkim və ya briqadanı seçərək göndərin.
                </div>
              )}
            </div>

            {/* Emergency Action Buttons */}
            <div className="space-y-2">
              <button
                onClick={handleAcknowledge}
                className="w-full flex items-center justify-center gap-2 py-3 px-4 bg-emerald-600 hover:bg-emerald-500 text-white font-bold rounded-xl shadow-lg shadow-emerald-950 transition text-xs sm:text-sm"
              >
                <CheckCircle2 className="w-5 h-5" />
                <span>Siqnalı Təsdiqlə və Dayandır (Acknowledge)</span>
              </button>

              <div className="grid grid-cols-2 gap-2 pt-0.5">
                <button
                  onClick={handleCallAmbulance}
                  disabled={callingAmbulance}
                  className="flex items-center justify-center gap-2 py-2.5 px-3 rounded-xl text-xs font-bold border transition bg-slate-800 hover:bg-slate-700 text-slate-100 border-slate-700 hover:border-rose-500 shadow-sm"
                >
                  <PhoneCall className="w-4 h-4 text-rose-400" />
                  <span>
                    {callingAmbulance
                      ? "Briqada Çağırılır..."
                      : ambulanceDispatched
                      ? "Briqadanı Yenidən Çağır"
                      : "Təcili Yardımı Çağır"}
                  </span>
                </button>

                <button
                  onClick={handleNotifyDoctor}
                  disabled={notifyingDoctor || !selectedDoctor}
                  className="flex items-center justify-center gap-2 py-2.5 px-3 rounded-xl text-xs font-bold border transition bg-slate-800 hover:bg-slate-700 text-slate-100 border-slate-700 hover:border-cyan-500 shadow-sm"
                >
                  <Send className="w-4 h-4 text-cyan-400" />
                  <span>
                    {notifyingDoctor
                      ? "Göndərilir..."
                      : doctorNotified
                      ? `Həkimə Göndər (${selectedDoctor?.cabinet_no || ""})`
                      : "Həkimə Bildiriş Göndər"}
                  </span>
                </button>
              </div>
            </div>
          </div>
        </div>
      ) : (
        /* Minimized Floating Alert Banner */
        <div className="fixed bottom-4 right-4 z-50 bg-rose-950/90 border border-rose-600 text-white p-3.5 rounded-xl shadow-2xl flex items-center gap-3 backdrop-blur-md animate-bounce">
          <AlertOctagon className="w-6 h-6 text-rose-400" />
          <div>
            <div className="text-xs font-bold text-rose-300">AKTİV TƏCİLİ HƏYƏCAN</div>
            <div className="text-xs font-mono">{currentAlert.location}</div>
          </div>
          <button
            onClick={() => setMinimized(false)}
            className="px-2.5 py-1 bg-rose-600 hover:bg-rose-500 text-white rounded text-xs font-bold transition"
          >
            Aç
          </button>
          <button
            onClick={handleAcknowledge}
            className="p-1 hover:bg-rose-900 rounded text-slate-300"
            title="Səsi dayandır"
          >
            <VolumeX className="w-4 h-4" />
          </button>
        </div>
      )}
    </>
  );
}
