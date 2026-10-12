"use client";

import React, { useState, useEffect } from "react";
import {
  Activity,
  AlertTriangle,
  Bell,
  Clock,
  Volume2,
  VolumeX,
  ShieldAlert,
  Radio,
  FileText,
  Building2,
  BarChart3,
  Flame,
  Monitor,
} from "lucide-react";
import { playEmergencyAlert, stopCurrentAlarm } from "@/lib/audioAlerts";

interface HeaderProps {
  activeTab: string;
  setActiveTab: (tab: string) => void;
  activeAlertCount: number;
  onTriggerPanic: (code: "CODE_BLUE" | "CODE_RED") => void;
  onOpenQueueMonitor: () => void;
}

export default function Header({
  activeTab,
  setActiveTab,
  activeAlertCount,
  onTriggerPanic,
  onOpenQueueMonitor,
}: HeaderProps) {
  const [time, setTime] = useState("");
  const [date, setDate] = useState("");
  const [isMuted, setIsMuted] = useState(false);
  const [audioTestActive, setAudioTestActive] = useState<string | null>(null);

  useEffect(() => {
    const updateTime = () => {
      const now = new Date();
      setTime(
        now.toLocaleTimeString("az-AZ", {
          hour: "2-digit",
          minute: "2-digit",
          second: "2-digit",
        })
      );
      setDate(
        now.toLocaleDateString("az-AZ", {
          weekday: "short",
          day: "2-digit",
          month: "short",
          year: "numeric",
        })
      );
    };
    updateTime();
    const interval = setInterval(updateTime, 1000);
    return () => clearInterval(interval);
  }, []);

  const handleTestAudio = (code: "CODE_BLUE" | "CODE_RED" | "CODE_YELLOW") => {
    setAudioTestActive(code);
    playEmergencyAlert(code);
    setTimeout(() => {
      setAudioTestActive(null);
      stopCurrentAlarm();
    }, 2800);
  };

  return (
    <header className="border-b border-slate-800 bg-[#0B0F19]/95 backdrop-blur-md sticky top-0 z-40 text-slate-100 shadow-xl">
      {/* Top Meta Bar */}
      <div className="flex flex-wrap items-center justify-between px-4 py-2 border-b border-slate-800/60 text-xs text-slate-400 gap-2">
        <div className="flex items-center space-x-3">
          <span className="flex items-center gap-1.5 text-emerald-400 font-semibold tracking-wide">
            <span className="w-2 h-2 rounded-full bg-emerald-500 animate-ping inline-block" />
            <span className="w-2 h-2 rounded-full bg-emerald-500 inline-block -ml-3.5" />
            SİSTEM AKTİVDİR • 24/7 TƏCİLİ XİDMƏT
          </span>
          <span className="text-slate-700">|</span>
          <span className="text-slate-300">Bakı Şəhər Təcili və Təxirəsalınmaz Tibbi Mərkəzi</span>
          <span className="text-slate-700 hidden md:inline">|</span>
          <span className="bg-slate-800 text-cyan-400 px-2 py-0.5 rounded text-[11px] font-mono border border-slate-700/80 hidden md:inline">
            Manchester Triage System (MTS) v5.2 • ISO 9001
          </span>
        </div>

        <div className="flex items-center space-x-3">
          {/* Live Queue Monitor Button */}
          <button
            onClick={onOpenQueueMonitor}
            className="flex items-center gap-1.5 px-2.5 py-1 bg-cyan-950/70 hover:bg-cyan-900/90 text-cyan-300 border border-cyan-800/80 rounded-lg text-xs font-bold transition shadow-sm"
            title="Gözləmə zalı üçün canlı növbə tablosunu aç"
          >
            <Monitor className="w-3.5 h-3.5 text-cyan-400" />
            <span>Növbə Tablosu (Monitor)</span>
          </button>

          {/* Live Baku Clock */}
          <div className="flex items-center gap-1.5 text-slate-200 font-mono bg-slate-900 px-2.5 py-1 rounded-lg border border-slate-800">
            <Clock className="w-3.5 h-3.5 text-cyan-400" />
            <span className="text-slate-400 text-[11px]">{date}</span>
            <span className="text-cyan-300 font-bold">{time}</span>
          </div>

          {/* Sound Controls */}
          <div className="flex items-center gap-1 bg-slate-900 p-0.5 rounded-lg border border-slate-800">
            <button
              onClick={() => {
                const nextState = !isMuted;
                setIsMuted(nextState);
                if (nextState) stopCurrentAlarm();
              }}
              title={isMuted ? "Səsi aktiv et" : "Səsi susdur"}
              className={`p-1.5 rounded-md transition ${
                isMuted
                  ? "bg-rose-500/20 text-rose-400"
                  : "bg-slate-800 text-slate-200 hover:text-white"
              }`}
            >
              {isMuted ? <VolumeX className="w-3.5 h-3.5" /> : <Volume2 className="w-3.5 h-3.5" />}
            </button>

            {/* Quick Sound Test buttons */}
            <div className="hidden sm:flex items-center gap-1 pr-1 text-[10px]">
              <button
                onClick={() => handleTestAudio("CODE_BLUE")}
                className={`px-1.5 py-0.5 rounded font-mono ${
                  audioTestActive === "CODE_BLUE"
                    ? "bg-blue-600 text-white animate-pulse"
                    : "bg-blue-950/60 text-blue-400 hover:bg-blue-900/80"
                }`}
              >
                Blue
              </button>
              <button
                onClick={() => handleTestAudio("CODE_RED")}
                className={`px-1.5 py-0.5 rounded font-mono ${
                  audioTestActive === "CODE_RED"
                    ? "bg-rose-600 text-white animate-pulse"
                    : "bg-rose-950/60 text-rose-400 hover:bg-rose-900/80"
                }`}
              >
                Red
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Main Bar */}
      <div className="px-4 py-2.5 flex flex-wrap items-center justify-between gap-4">
        {/* Brand */}
        <div className="flex items-center space-x-3">
          <div className="w-10 h-10 rounded-xl bg-gradient-to-tr from-cyan-600 to-blue-500 p-0.5 shadow-lg shadow-cyan-900/30">
            <div className="w-full h-full bg-slate-950 rounded-[10px] flex items-center justify-center">
              <Activity className="w-5 h-5 text-cyan-400 animate-ecg-heartbeat" />
            </div>
          </div>
          <div>
            <div className="flex items-center gap-2">
              <h1 className="text-lg font-black tracking-wider bg-clip-text text-transparent bg-gradient-to-r from-cyan-400 via-blue-200 to-slate-100">
                MEDTRIAGE PRO
              </h1>
              <span className="bg-rose-500/20 text-rose-400 border border-rose-500/30 text-[10px] font-bold px-1.5 py-0.2 rounded uppercase tracking-wider">
                Enterprise v3.5
              </span>
            </div>
            <p className="text-[11px] text-slate-400">
              Klinik Qəbul, Ekspert Diaqnostika və Təcili Həyəcan Terminalı
            </p>
          </div>
        </div>

        {/* Navigation Tabs */}
        <nav className="flex items-center bg-slate-900 p-1 rounded-xl border border-slate-800 space-x-1">
          <button
            onClick={() => setActiveTab("reception")}
            className={`flex items-center gap-2 px-3.5 py-2 rounded-lg text-xs font-semibold transition ${
              activeTab === "reception"
                ? "bg-gradient-to-r from-cyan-600 to-blue-600 text-white shadow-md shadow-cyan-950/50"
                : "text-slate-400 hover:text-slate-200 hover:bg-slate-800/60"
            }`}
          >
            <FileText className="w-4 h-4" />
            <span>Qəbul və Triaq (CDSS)</span>
          </button>

          <button
            onClick={() => setActiveTab("emergency")}
            className={`flex items-center gap-2 px-3.5 py-2 rounded-lg text-xs font-semibold transition relative ${
              activeTab === "emergency"
                ? "bg-rose-600 text-white shadow-md shadow-rose-950/50"
                : "text-slate-400 hover:text-slate-200 hover:bg-slate-800/60"
            }`}
          >
            <ShieldAlert className="w-4 h-4" />
            <span>Təcili Terminal</span>
            {activeAlertCount > 0 && (
              <span className="w-4 h-4 rounded-full bg-rose-500 text-white text-[10px] flex items-center justify-center font-bold animate-ping absolute -top-1 -right-1">
                {activeAlertCount}
              </span>
            )}
            {activeAlertCount > 0 && (
              <span className="w-4 h-4 rounded-full bg-rose-500 text-white text-[10px] flex items-center justify-center font-bold ml-1">
                {activeAlertCount}
              </span>
            )}
          </button>

          <button
            onClick={() => setActiveTab("cabinets")}
            className={`flex items-center gap-2 px-3.5 py-2 rounded-lg text-xs font-semibold transition ${
              activeTab === "cabinets"
                ? "bg-gradient-to-r from-cyan-600 to-blue-600 text-white shadow-md shadow-cyan-950/50"
                : "text-slate-400 hover:text-slate-200 hover:bg-slate-800/60"
            }`}
          >
            <Building2 className="w-4 h-4" />
            <span>Həkim Kabinetləri</span>
          </button>

          <button
            onClick={() => setActiveTab("audit")}
            className={`flex items-center gap-2 px-3.5 py-2 rounded-lg text-xs font-semibold transition ${
              activeTab === "audit"
                ? "bg-gradient-to-r from-cyan-600 to-blue-600 text-white shadow-md shadow-cyan-950/50"
                : "text-slate-400 hover:text-slate-200 hover:bg-slate-800/60"
            }`}
          >
            <BarChart3 className="w-4 h-4" />
            <span>Audit & Statistika</span>
          </button>
        </nav>

        {/* Rapid Panic Alert Triggers */}
        <div className="flex items-center space-x-2">
          <button
            onClick={() => onTriggerPanic("CODE_RED")}
            className="flex items-center gap-1.5 px-3 py-1.5 bg-rose-600/20 hover:bg-rose-600 text-rose-300 hover:text-white border border-rose-500/40 rounded-lg text-xs font-bold transition shadow-sm"
            title="Kəskin Təcili Vəziyyət (İnfarct, İnsult, Politravma)"
          >
            <Flame className="w-3.5 h-3.5" />
            <span>CODE RED</span>
          </button>
          <button
            onClick={() => onTriggerPanic("CODE_BLUE")}
            className="flex items-center gap-1.5 px-3 py-1.5 bg-blue-600/20 hover:bg-blue-600 text-blue-300 hover:text-white border border-blue-500/40 rounded-lg text-xs font-bold transition shadow-sm"
            title="Reanimasiya / Klinik Ölüm Təhlükəsi"
          >
            <Radio className="w-3.5 h-3.5" />
            <span>CODE BLUE</span>
          </button>
        </div>
      </div>
    </header>
  );
}
