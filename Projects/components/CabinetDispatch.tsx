"use client";

import React, { useState, useEffect } from "react";
import {
  Building2,
  Stethoscope,
  Phone,
  User,
  CheckCircle2,
  AlertCircle,
  Clock,
  RefreshCw,
  Search,
  Filter,
  Megaphone,
  UserPlus,
  ArrowRight,
  ShieldAlert,
  Check,
  DoorOpen,
} from "lucide-react";
import { playHospitalCallChime } from "@/lib/audioAlerts";

interface Cabinet {
  id: number;
  cabinet_no: string;
  department: string;
  doctor_name: string;
  doctor_specialty: string;
  status: "Boşdur" | "Xəstə qəbulunda" | "Təcili çağırış" | "Fasilə";
  current_patient_ticket: string | null;
  current_patient_name?: string | null;
  current_patient_level?: number | null;
  current_patient_color?: string | null;
  floor: string;
  phone_ext: string;
  updated_at: string;
}

interface Department {
  id: number;
  code: string;
  name: string;
  head_doctor: string;
  floor: string;
  active_cases: number;
}

interface WaitingPatient {
  id: number;
  ticket_number: string;
  full_name: string;
  triage_level: number;
  triage_color: string;
  assigned_department: string;
}

export default function CabinetDispatch() {
  const [cabinets, setCabinets] = useState<Cabinet[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [waitingPatients, setWaitingPatients] = useState<WaitingPatient[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedDept, setSelectedDept] = useState("ALL");
  const [statusFilter, setStatusFilter] = useState("ALL");
  const [callingCabinet, setCallingCabinet] = useState<string | null>(null);
  const [lastCalledAnnouncement, setLastCalledAnnouncement] = useState<{
    ticket: string;
    name: string;
    cabinet: string;
  } | null>(null);

  // Manual assign modal state
  const [assignModalCabinet, setAssignModalCabinet] = useState<Cabinet | null>(null);

  const fetchCabinetsAndQueue = async () => {
    try {
      setLoading(true);
      const [cRes, pRes] = await Promise.all([
        fetch("/api/cabinets"),
        fetch("/api/patients?status=Gözləyir"),
      ]);

      const cJson = await cRes.json();
      const pJson = await pRes.json();

      if (cJson.success) {
        setCabinets(cJson.cabinets);
        setDepartments(cJson.departments);
      }
      if (pJson.success) {
        setWaitingPatients(pJson.data);
      }
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCabinetsAndQueue();
    const interval = setInterval(fetchCabinetsAndQueue, 8000);
    return () => clearInterval(interval);
  }, []);

  // Call Next Patient into Cabinet
  const handleCallNext = async (cab: Cabinet) => {
    try {
      setCallingCabinet(cab.cabinet_no);
      const res = await fetch("/api/cabinets/call-next", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          cabinetNo: cab.cabinet_no,
          department: cab.department,
        }),
      });

      const json = await res.json();
      if (json.success && json.patient) {
        playHospitalCallChime();
        setLastCalledAnnouncement({
          ticket: json.patient.ticket_number,
          name: json.patient.full_name,
          cabinet: cab.cabinet_no,
        });
        setTimeout(() => setLastCalledAnnouncement(null), 8000);
        await fetchCabinetsAndQueue();
      } else {
        alert(json.message || "Növbədə gözləyən pasiyent tapılmadı.");
      }
    } catch (err) {
      console.error(err);
    } finally {
      setCallingCabinet(null);
    }
  };

  // Complete consultation / free up cabinet
  const handleFinishConsultation = async (cab: Cabinet) => {
    try {
      await fetch("/api/cabinets", {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          cabinetNo: cab.cabinet_no,
          status: "Boşdur",
          currentPatientTicket: cab.current_patient_ticket,
        }),
      });
      await fetchCabinetsAndQueue();
    } catch (err) {
      console.error(err);
    }
  };

  // Switch status
  const updateStatus = async (
    cabinetNo: string,
    status: "Boşdur" | "Xəstə qəbulunda" | "Təcili çağırış" | "Fasilə",
    ticket?: string | null
  ) => {
    try {
      await fetch("/api/cabinets", {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          cabinetNo,
          status,
          currentPatientTicket: ticket,
        }),
      });
      await fetchCabinetsAndQueue();
    } catch (err) {
      console.error(err);
    }
  };

  // Assign specific patient from modal
  const handleAssignSpecific = async (patient: WaitingPatient) => {
    if (!assignModalCabinet) return;
    try {
      await fetch("/api/cabinets", {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          cabinetNo: assignModalCabinet.cabinet_no,
          status: "Xəstə qəbulunda",
          currentPatientTicket: patient.ticket_number,
        }),
      });
      playHospitalCallChime();
      setLastCalledAnnouncement({
        ticket: patient.ticket_number,
        name: patient.full_name,
        cabinet: assignModalCabinet.cabinet_no,
      });
      setAssignModalCabinet(null);
      await fetchCabinetsAndQueue();
    } catch (err) {
      console.error(err);
    }
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case "Boşdur":
        return "bg-emerald-500/20 text-emerald-400 border-emerald-500/40";
      case "Xəstə qəbulunda":
        return "bg-amber-500/20 text-amber-300 border-amber-500/40";
      case "Təcili çağırış":
        return "bg-rose-500/20 text-rose-300 border-rose-500/40 animate-pulse";
      case "Fasilə":
        return "bg-slate-700/40 text-slate-400 border-slate-600";
      default:
        return "bg-slate-800 text-slate-300 border-slate-700";
    }
  };

  const filteredCabinets = cabinets.filter((c) => {
    if (selectedDept !== "ALL" && c.department !== selectedDept) return false;
    if (statusFilter !== "ALL" && c.status !== statusFilter) return false;
    return true;
  });

  return (
    <div className="space-y-6">
      {/* Live Hospital Public Announcement Banner */}
      {lastCalledAnnouncement && (
        <div className="bg-gradient-to-r from-cyan-900/90 via-blue-900/90 to-slate-900 p-4 rounded-2xl border-2 border-cyan-400 text-white flex items-center justify-between shadow-2xl animate-bounce">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 rounded-full bg-cyan-500 text-slate-950 flex items-center justify-center font-bold">
              <Megaphone className="w-5 h-5" />
            </div>
            <div>
              <div className="text-[10px] uppercase font-bold tracking-widest text-cyan-300">
                KLİNİK AUDİO ÇAĞIRIŞ EDİLDİ
              </div>
              <div className="text-base font-black">
                Talon: <span className="font-mono text-cyan-200">{lastCalledAnnouncement.ticket}</span> (
                {lastCalledAnnouncement.name}) — Zəhmət olmasa{" "}
                <span className="text-amber-300 font-mono underline">
                  {lastCalledAnnouncement.cabinet}
                </span>{" "}
                kabinetinə yaxınlaşın!
              </div>
            </div>
          </div>
          <button
            onClick={() => setLastCalledAnnouncement(null)}
            className="text-xs text-slate-400 hover:text-white px-3 py-1 bg-slate-950/60 rounded-lg"
          >
            Bağla
          </button>
        </div>
      )}

      {/* Control & Dispatch Bar */}
      <div className="flex flex-wrap items-center justify-between gap-4 bg-slate-900/90 p-4 rounded-2xl border border-slate-800">
        <div>
          <div className="flex items-center gap-2">
            <Building2 className="w-5 h-5 text-cyan-400" />
            <h2 className="text-base font-bold text-white">
              Həkim Kabinetləri və Operativ Dispetçer İdarəetməsi
            </h2>
          </div>
          <p className="text-xs text-slate-400 mt-0.5">
            MTS prioritetinə əsaslanan real vaxt növbə çağırışı və həkim məşğulluq terminalı
          </p>
        </div>

        <div className="flex flex-wrap items-center gap-2">
          {/* Department filter */}
          <select
            value={selectedDept}
            onChange={(e) => setSelectedDept(e.target.value)}
            className="bg-slate-950 border border-slate-800 rounded-lg px-3 py-1.5 text-xs text-slate-200 focus:outline-none focus:border-cyan-500"
          >
            <option value="ALL">Bütün Şöbələr ({cabinets.length})</option>
            {departments.map((d) => (
              <option key={d.id} value={d.name}>
                {d.name}
              </option>
            ))}
          </select>

          {/* Status filter */}
          <select
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
            className="bg-slate-950 border border-slate-800 rounded-lg px-3 py-1.5 text-xs text-slate-200 focus:outline-none focus:border-cyan-500"
          >
            <option value="ALL">Bütün Statuslar</option>
            <option value="Boşdur">Boşdur</option>
            <option value="Xəstə qəbulunda">Xəstə qəbulunda</option>
            <option value="Təcili çağırış">Təcili çağırış</option>
            <option value="Fasilə">Fasilə</option>
          </select>

          <button
            onClick={fetchCabinetsAndQueue}
            className="p-2 bg-slate-800 hover:bg-slate-700 text-slate-300 rounded-lg transition"
            title="Yenilə"
          >
            <RefreshCw className={`w-4 h-4 ${loading ? "animate-spin text-cyan-400" : ""}`} />
          </button>
        </div>
      </div>

      {/* Departments Summary Metrics */}
      <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-3">
        {departments.map((dept) => (
          <div
            key={dept.id}
            onClick={() => setSelectedDept(selectedDept === dept.name ? "ALL" : dept.name)}
            className={`p-3 rounded-xl border cursor-pointer transition ${
              selectedDept === dept.name
                ? "bg-cyan-950/40 border-cyan-500 shadow-md shadow-cyan-950"
                : "bg-slate-900/60 border-slate-800/80 hover:bg-slate-800/50"
            }`}
          >
            <div className="text-[10px] font-mono text-cyan-400 uppercase font-semibold">
              {dept.code}
            </div>
            <div className="text-xs font-bold text-white mt-0.5 truncate">{dept.name}</div>
            <div className="text-[10px] text-slate-400 mt-1 truncate">{dept.head_doctor}</div>
            <div className="mt-2 flex items-center justify-between text-[10px] pt-1.5 border-t border-slate-800">
              <span className="text-slate-500">{dept.floor}</span>
              <span className="bg-slate-800 px-1.5 py-0.2 rounded font-mono text-cyan-300 font-bold">
                {dept.active_cases} xəstə
              </span>
            </div>
          </div>
        ))}
      </div>

      {/* Cabinets Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        {filteredCabinets.map((cab) => {
          const isBusy = cab.status === "Xəstə qəbulunda";
          const isEmergency = cab.status === "Təcili çağırış";

          return (
            <div
              key={cab.id}
              className={`bg-slate-900/90 rounded-2xl p-4 border flex flex-col justify-between space-y-4 transition ${
                isEmergency
                  ? "border-rose-500/80 shadow-lg shadow-rose-950/50 bg-rose-950/10"
                  : isBusy
                  ? "border-amber-500/40"
                  : "border-slate-800 hover:border-slate-700"
              }`}
            >
              {/* Header */}
              <div>
                <div className="flex items-center justify-between pb-2 border-b border-slate-800">
                  <div className="flex items-center gap-2">
                    <span className="text-lg font-black font-mono text-cyan-300">
                      {cab.cabinet_no}
                    </span>
                    <span className="text-[10px] text-slate-400 bg-slate-800 px-2 py-0.5 rounded font-mono">
                      {cab.floor}
                    </span>
                  </div>
                  <span
                    className={`text-[10px] font-bold px-2.5 py-0.5 rounded-full border ${getStatusBadge(
                      cab.status
                    )}`}
                  >
                    {cab.status}
                  </span>
                </div>

                {/* Doctor Info */}
                <div className="mt-3 space-y-1 text-xs">
                  <div className="text-slate-400 text-[11px] truncate">{cab.department}</div>
                  <div className="text-white font-bold text-sm flex items-center gap-1.5">
                    <Stethoscope className="w-4 h-4 text-cyan-400 shrink-0" />
                    <span className="truncate">{cab.doctor_name}</span>
                  </div>
                  <div className="text-slate-400 text-[11px]">{cab.doctor_specialty}</div>
                  <div className="flex items-center gap-1 text-[11px] text-slate-500 pt-0.5">
                    <Phone className="w-3 h-3" />
                    <span>Daxili: {cab.phone_ext}</span>
                  </div>
                </div>

                {/* Current Active Patient Box */}
                <div className="mt-4 p-3 bg-slate-950 rounded-xl border border-slate-800 text-xs">
                  <div className="text-[10px] text-slate-500 uppercase font-semibold flex items-center justify-between">
                    <span>Qəbul Edilən Pasiyent:</span>
                    {isBusy && <span className="text-amber-400 font-mono animate-pulse">● Canlı Qəbul</span>}
                  </div>

                  {cab.current_patient_ticket ? (
                    <div className="mt-1.5">
                      <div className="flex items-center justify-between">
                        <span className="font-mono font-black text-cyan-300 text-sm">
                          {cab.current_patient_ticket}
                        </span>
                        {cab.current_patient_level && (
                          <span className="text-[10px] px-1.5 py-0.2 rounded font-bold bg-slate-800 text-slate-200">
                            L{cab.current_patient_level}
                          </span>
                        )}
                      </div>
                      <div className="text-slate-200 font-semibold truncate text-xs mt-0.5">
                        {cab.current_patient_name || "Xəstə qəbulu aparılır"}
                      </div>
                    </div>
                  ) : (
                    <span className="text-slate-500 italic mt-1 block">
                      Hazırda xəstə yoxdur (Kabinet boşdur)
                    </span>
                  )}
                </div>
              </div>

              {/* Action Buttons */}
              <div className="space-y-2 pt-2 border-t border-slate-800">
                {/* 1. Main Action: Call Next Patient (MTS Priority Queue) */}
                <button
                  type="button"
                  disabled={callingCabinet === cab.cabinet_no}
                  onClick={() => handleCallNext(cab)}
                  className="w-full flex items-center justify-center gap-2 py-2 px-3 bg-gradient-to-r from-cyan-600 to-blue-600 hover:from-cyan-500 hover:to-blue-500 text-white font-bold rounded-xl text-xs shadow-md shadow-cyan-950 transition disabled:opacity-50"
                >
                  <Megaphone className="w-3.5 h-3.5" />
                  <span>
                    {callingCabinet === cab.cabinet_no ? "Çağırılır..." : "Növbəti Pasiyenti Çağır"}
                  </span>
                </button>

                {/* Secondary row */}
                <div className="grid grid-cols-2 gap-1.5 text-[10px]">
                  {isBusy ? (
                    <button
                      type="button"
                      onClick={() => handleFinishConsultation(cab)}
                      className="p-1.5 rounded font-bold text-center bg-emerald-600 hover:bg-emerald-500 text-white transition flex items-center justify-center gap-1"
                    >
                      <Check className="w-3 h-3" />
                      <span>Qəbulu Bitir</span>
                    </button>
                  ) : (
                    <button
                      type="button"
                      onClick={() => setAssignModalCabinet(cab)}
                      className="p-1.5 rounded font-medium text-center bg-slate-800 hover:bg-slate-700 text-slate-200 transition flex items-center justify-center gap-1 border border-slate-700"
                    >
                      <UserPlus className="w-3 h-3" />
                      <span>Növbədən Seç</span>
                    </button>
                  )}

                  <button
                    type="button"
                    onClick={() =>
                      updateStatus(
                        cab.cabinet_no,
                        cab.status === "Fasilə" ? "Boşdur" : "Fasilə",
                        cab.current_patient_ticket
                      )
                    }
                    className={`p-1.5 rounded font-medium text-center border transition ${
                      cab.status === "Fasilə"
                        ? "bg-slate-700 text-white border-slate-600"
                        : "bg-slate-950 text-slate-400 border-slate-800 hover:bg-slate-800"
                    }`}
                  >
                    {cab.status === "Fasilə" ? "Fasiləni Bitir" : "Fasilə Ver"}
                  </button>
                </div>
              </div>
            </div>
          );
        })}
      </div>

      {/* Modal: Select specific waiting patient */}
      {assignModalCabinet && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/80 backdrop-blur-sm">
          <div className="w-full max-w-lg bg-slate-900 border border-slate-800 rounded-2xl p-5 shadow-2xl text-xs space-y-4">
            <div className="flex items-center justify-between border-b border-slate-800 pb-3">
              <div>
                <span className="text-cyan-400 font-mono font-bold">
                  {assignModalCabinet.cabinet_no} — {assignModalCabinet.department}
                </span>
                <h3 className="text-base font-bold text-white mt-0.5">
                  Gözləyən Pasiyentlər Siyahısından Təyin Et
                </h3>
              </div>
              <button
                onClick={() => setAssignModalCabinet(null)}
                className="text-slate-400 hover:text-white p-1 rounded"
              >
                ✕
              </button>
            </div>

            <div className="max-h-[300px] overflow-y-auto space-y-2 pr-1">
              {waitingPatients.length > 0 ? (
                waitingPatients.map((p) => (
                  <div
                    key={p.id}
                    onClick={() => handleAssignSpecific(p)}
                    className="p-3 bg-slate-950 hover:bg-slate-800/80 rounded-xl border border-slate-800 cursor-pointer flex items-center justify-between transition"
                  >
                    <div>
                      <div className="flex items-center gap-2">
                        <span className="font-mono font-bold text-cyan-300">{p.ticket_number}</span>
                        <span className="text-[10px] px-1.5 py-0.2 rounded font-bold bg-slate-800 text-slate-300">
                          L{p.triage_level}: {p.triage_color}
                        </span>
                      </div>
                      <div className="font-bold text-white mt-0.5">{p.full_name}</div>
                      <div className="text-[10px] text-slate-400">{p.assigned_department}</div>
                    </div>
                    <ArrowRight className="w-4 h-4 text-cyan-400" />
                  </div>
                ))
              ) : (
                <div className="p-6 text-center text-slate-500">
                  Hazırda gözləyən aktiv pasiyent yoxdur.
                </div>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
