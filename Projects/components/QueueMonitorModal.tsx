"use client";

import React, { useState, useEffect } from "react";
import {
  Monitor,
  X,
  Volume2,
  Clock,
  Activity,
  Megaphone,
  UserCheck,
  CheckCircle,
  Building,
} from "lucide-react";

interface QueuePatient {
  id: number;
  ticket_number: string;
  full_name: string;
  triage_level: number;
  triage_color: string;
  status: string;
  assigned_department: string;
  assigned_cabinet: string;
  registered_at: string;
}

interface CabinetInfo {
  cabinet_no: string;
  department: string;
  doctor_name: string;
  status: string;
  current_patient_ticket: string | null;
  current_patient_name?: string | null;
}

interface QueueMonitorModalProps {
  isOpen: boolean;
  onClose: () => void;
}

export default function QueueMonitorModal({ isOpen, onClose }: QueueMonitorModalProps) {
  const [callingCabinets, setCallingCabinets] = useState<CabinetInfo[]>([]);
  const [waitingQueue, setWaitingQueue] = useState<QueuePatient[]>([]);
  const [time, setTime] = useState("");

  const loadData = async () => {
    try {
      const [cRes, pRes] = await Promise.all([
        fetch("/api/cabinets"),
        fetch("/api/patients?status=Gözləyir"),
      ]);
      const cJson = await cRes.json();
      const pJson = await pRes.json();
      if (cJson.success) {
        setCallingCabinets(cJson.cabinets);
      }
      if (pJson.success) {
        setWaitingQueue(pJson.data);
      }
    } catch (e) {
      console.error(e);
    }
  };

  useEffect(() => {
    if (!isOpen) return;
    loadData();
    const interval = setInterval(loadData, 5000);
    const timeTimer = setInterval(() => {
      setTime(new Date().toLocaleTimeString("az-AZ"));
    }, 1000);
    return () => {
      clearInterval(interval);
      clearInterval(timeTimer);
    };
  }, [isOpen]);

  if (!isOpen) return null;

  const activeCalls = callingCabinets.filter(
    (c) => c.status === "Xəstə qəbulunda" && c.current_patient_ticket
  );

  return (
    <div className="fixed inset-0 z-50 bg-[#06090E]/98 flex flex-col p-6 backdrop-blur-xl text-white">
      {/* Top Monitor Header */}
      <div className="flex items-center justify-between pb-4 border-b border-slate-800">
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-xl bg-cyan-500/20 text-cyan-400 flex items-center justify-center border border-cyan-500/40">
            <Monitor className="w-6 h-6" />
          </div>
          <div>
            <h1 className="text-xl font-black tracking-wider bg-clip-text text-transparent bg-gradient-to-r from-cyan-400 to-blue-200">
              MEDTRIAGE PRO — CANLI ÇAĞIRIŞ VƏ NÖVBƏ MONİTORU
            </h1>
            <p className="text-xs text-slate-400">
              Bakı Klinik Mərkəzi • İctimai Qəbul və Gözləmə Zalı İnformasiya Tablosu
            </p>
          </div>
        </div>

        <div className="flex items-center gap-4">
          <div className="font-mono text-2xl font-black text-cyan-300 bg-slate-900 px-4 py-1.5 rounded-xl border border-slate-800">
            {time}
          </div>
          <button
            onClick={onClose}
            className="p-2 bg-slate-800 hover:bg-slate-700 text-slate-300 hover:text-white rounded-xl transition"
          >
            <X className="w-6 h-6" />
          </button>
        </div>
      </div>

      {/* Main Screen: Split between Called Now & Waiting List */}
      <div className="grid grid-cols-1 lg:grid-cols-12 gap-6 flex-1 mt-6 overflow-hidden">
        {/* Left Column: Currently Calling to Cabinets (Cols 7) */}
        <div className="lg:col-span-7 bg-slate-900/80 rounded-2xl p-5 border border-slate-800 flex flex-col">
          <div className="flex items-center gap-2 pb-3 border-b border-slate-800">
            <Megaphone className="w-5 h-5 text-amber-400" />
            <h2 className="text-base font-bold uppercase tracking-wider text-amber-300">
              İndi Qəbula Dəvət Olunanlar (Çağırış)
            </h2>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-4 overflow-y-auto flex-1">
            {activeCalls.length > 0 ? (
              activeCalls.map((cab) => (
                <div
                  key={cab.cabinet_no}
                  className="bg-gradient-to-br from-slate-950 to-slate-900 p-5 rounded-2xl border-2 border-amber-500/80 shadow-xl shadow-amber-950/20 flex flex-col justify-between"
                >
                  <div>
                    <div className="flex items-center justify-between text-xs text-slate-400 pb-2 border-b border-slate-800">
                      <span className="font-bold text-cyan-400">{cab.department}</span>
                      <span className="font-mono bg-slate-800 px-2 py-0.5 rounded text-slate-200">
                        {cab.cabinet_no}
                      </span>
                    </div>

                    <div className="my-4 text-center">
                      <div className="text-[10px] text-slate-500 uppercase tracking-widest font-semibold">
                        Pasiyent Talonu
                      </div>
                      <div className="text-4xl font-black font-mono text-amber-300 tracking-tight mt-1">
                        {cab.current_patient_ticket}
                      </div>
                      <div className="text-sm font-bold text-white mt-1 truncate">
                        {cab.current_patient_name || "Xəstə qəbulu"}
                      </div>
                    </div>
                  </div>

                  <div className="pt-3 border-t border-slate-800/80 text-xs text-slate-400 flex items-center justify-between">
                    <span>Həkim: {cab.doctor_name}</span>
                    <span className="text-emerald-400 font-bold">Qəbulda</span>
                  </div>
                </div>
              ))
            ) : (
              <div className="col-span-2 flex items-center justify-center p-12 text-slate-500 text-sm">
                Hazırda aktiv çağırılan xəstə yoxdur.
              </div>
            )}
          </div>
        </div>

        {/* Right Column: Waiting Queue (Cols 5) */}
        <div className="lg:col-span-5 bg-slate-900/80 rounded-2xl p-5 border border-slate-800 flex flex-col">
          <div className="flex items-center justify-between pb-3 border-b border-slate-800">
            <div className="flex items-center gap-2">
              <Clock className="w-5 h-5 text-cyan-400" />
              <h2 className="text-base font-bold uppercase tracking-wider text-cyan-300">
                Növbədə Gözləyənlər ({waitingQueue.length})
              </h2>
            </div>
            <span className="text-xs text-slate-400 font-mono">MTS Prioriteti</span>
          </div>

          <div className="space-y-2.5 mt-4 overflow-y-auto flex-1 pr-1">
            {waitingQueue.length > 0 ? (
              waitingQueue.map((p, idx) => (
                <div
                  key={p.id}
                  className="bg-slate-950 p-3.5 rounded-xl border border-slate-800/80 flex items-center justify-between"
                >
                  <div className="flex items-center gap-3">
                    <span className="w-6 text-center font-mono text-slate-500 text-xs font-bold">
                      #{idx + 1}
                    </span>
                    <div>
                      <div className="flex items-center gap-2">
                        <span className="font-mono font-black text-cyan-300 text-base">
                          {p.ticket_number}
                        </span>
                        <span
                          className={`text-[10px] px-2 py-0.5 rounded-full font-bold uppercase ${
                            p.triage_level === 1
                              ? "bg-rose-600 text-white animate-pulse"
                              : p.triage_level === 2
                              ? "bg-orange-600 text-white"
                              : p.triage_level === 3
                              ? "bg-amber-500 text-slate-950"
                              : "bg-emerald-600 text-white"
                          }`}
                        >
                          L{p.triage_level}: {p.triage_color}
                        </span>
                      </div>
                      <div className="text-xs font-semibold text-slate-300 mt-0.5 truncate max-w-[200px]">
                        {p.full_name}
                      </div>
                    </div>
                  </div>

                  <div className="text-right text-[11px]">
                    <span className="text-slate-400 block">{p.assigned_department}</span>
                    <span className="text-slate-500 font-mono text-[10px]">
                      {new Date(p.registered_at).toLocaleTimeString("az-AZ")}
                    </span>
                  </div>
                </div>
              ))
            ) : (
              <div className="p-8 text-center text-slate-500 text-sm">
                Növbədə gözləyən xəstə yoxdur.
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
