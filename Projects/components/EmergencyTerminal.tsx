"use client";

import React, { useState, useEffect } from "react";
import {
  ShieldAlert,
  Flame,
  Radio,
  Bell,
  CheckCircle2,
  Clock,
  MapPin,
  AlertTriangle,
  Volume2,
  VolumeX,
  Play,
  Square,
  RefreshCw,
  PlusCircle,
} from "lucide-react";
import {
  playEmergencyAlert,
  stopCurrentAlarm,
  getActiveAlarmCode,
} from "@/lib/audioAlerts";
import { EmergencyAlertItem } from "./EmergencyOverlay";

interface EmergencyTerminalProps {
  onRefreshNeeded: () => void;
}

export default function EmergencyTerminal({
  onRefreshNeeded,
}: EmergencyTerminalProps) {
  const [activeAlerts, setActiveAlerts] = useState<EmergencyAlertItem[]>([]);
  const [recentAlerts, setRecentAlerts] = useState<EmergencyAlertItem[]>([]);
  const [recentDispatches, setRecentDispatches] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentPlaying, setCurrentPlaying] = useState<string | null>(null);

  // Manual Trigger Form
  const [newCodeType, setNewCodeType] = useState<"CODE_BLUE" | "CODE_RED" | "CODE_YELLOW">("CODE_RED");
  const [newLocation, setNewLocation] = useState("Kardiologiya / Şok Zalı (Mərtəbə 1)");
  const [newReason, setNewReason] = useState("Kəskin tənəffüs və qan dövranı kollapsı");
  const [newPatientName, setNewPatientName] = useState("");
  const [triggering, setTriggering] = useState(false);

  const fetchAlerts = async () => {
    try {
      setLoading(true);
      const res = await fetch("/api/alerts");
      const json = await res.json();
      if (json.success) {
        setActiveAlerts(json.activeAlerts);
        setRecentAlerts(json.recentAlerts);
        setRecentDispatches(json.recentDispatches || []);
      }
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAlerts();
    const interval = setInterval(fetchAlerts, 5000);
    return () => clearInterval(interval);
  }, []);

  const handleManualTrigger = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      setTriggering(true);
      const res = await fetch("/api/alerts", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          codeType: newCodeType,
          location: newLocation,
          reason: newReason,
          patientName: newPatientName,
        }),
      });
      if (res.ok) {
        playEmergencyAlert(newCodeType);
        await fetchAlerts();
        onRefreshNeeded();
      }
    } catch (err) {
      console.error(err);
    } finally {
      setTriggering(false);
    }
  };

  const handleAcknowledge = async (id: number) => {
    try {
      stopCurrentAlarm();
      setCurrentPlaying(null);
      await fetch("/api/alerts", {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          alertId: id,
          action: "ACKNOWLEDGE",
          acknowledgedBy: "Növbətçi Triaq Həkimi",
        }),
      });
      await fetchAlerts();
      onRefreshNeeded();
    } catch (err) {
      console.error(err);
    }
  };

  const handleResolve = async (id: number) => {
    try {
      await fetch("/api/alerts", {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          alertId: id,
          action: "RESOLVE",
        }),
      });
      await fetchAlerts();
      onRefreshNeeded();
    } catch (err) {
      console.error(err);
    }
  };

  const toggleSoundTest = (code: "CODE_BLUE" | "CODE_RED" | "CODE_YELLOW") => {
    if (currentPlaying === code) {
      stopCurrentAlarm();
      setCurrentPlaying(null);
    } else {
      playEmergencyAlert(code);
      setCurrentPlaying(code);
    }
  };

  return (
    <div className="space-y-6">
      {/* Top Banner */}
      <div className="bg-slate-900/90 rounded-2xl p-4 border border-slate-800 flex flex-wrap items-center justify-between gap-4">
        <div>
          <h2 className="text-base font-bold text-white flex items-center gap-2">
            <ShieldAlert className="w-5 h-5 text-rose-500" />
            <span>Təcili Həyəcan İdarəetmə Mərkəzi (Emergency Dispatch)</span>
          </h2>
          <p className="text-xs text-slate-400 mt-0.5">
            Xəstəxana daxili Code Blue, Code Red, Code Yellow siqnallarının monitorinqi və Web Audio generatoru
          </p>
        </div>

        <div className="flex items-center gap-2">
          {/* Audio Stop Button */}
          <button
            onClick={() => {
              stopCurrentAlarm();
              setCurrentPlaying(null);
            }}
            className="flex items-center gap-1.5 px-3 py-1.5 bg-slate-800 hover:bg-slate-700 text-slate-200 rounded-lg text-xs font-semibold border border-slate-700 transition"
          >
            <Square className="w-3.5 h-3.5 text-rose-400" />
            <span>Bütün Səsləri Dayandır</span>
          </button>

          <button
            onClick={fetchAlerts}
            className="p-1.5 bg-slate-800 hover:bg-slate-700 text-slate-300 rounded-lg transition"
            title="Yenilə"
          >
            <RefreshCw className={`w-4 h-4 ${loading ? "animate-spin text-cyan-400" : ""}`} />
          </button>
        </div>
      </div>

      {/* Grid: Audio Synthesizer Cards */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        {/* Code Blue Card */}
        <div className="bg-slate-900/90 rounded-2xl p-4 border border-blue-500/40 shadow-lg shadow-blue-950/40 flex flex-col justify-between">
          <div>
            <div className="flex items-center justify-between">
              <span className="px-2.5 py-0.5 rounded-full bg-blue-600 text-white text-[10px] font-black uppercase tracking-wider">
                CODE BLUE
              </span>
              <span className="text-xs font-mono text-blue-400">1000 Hz Pulsasiya</span>
            </div>
            <h3 className="text-sm font-bold text-white mt-2">Reanimasiya / Klinik Ölüm</h3>
            <p className="text-xs text-slate-400 mt-1">
              Asfiksiya, tənəffüs və ürək fəaliyyətinin qəfil dayanması. Yüksək tezlikli 1000 Hz aqressiv siqnal.
            </p>
          </div>

          <button
            onClick={() => toggleSoundTest("CODE_BLUE")}
            className={`mt-4 w-full flex items-center justify-center gap-2 py-2 px-3 rounded-xl text-xs font-bold transition ${
              currentPlaying === "CODE_BLUE"
                ? "bg-blue-600 text-white animate-pulse"
                : "bg-blue-950/60 hover:bg-blue-900/80 text-blue-300 border border-blue-800"
            }`}
          >
            {currentPlaying === "CODE_BLUE" ? <Square className="w-3.5 h-3.5" /> : <Play className="w-3.5 h-3.5" />}
            <span>{currentPlaying === "CODE_BLUE" ? "Dayandır" : "Səs Siqnalını Sına (1000 Hz)"}</span>
          </button>
        </div>

        {/* Code Red Card */}
        <div className="bg-slate-900/90 rounded-2xl p-4 border border-rose-500/40 shadow-lg shadow-rose-950/40 flex flex-col justify-between">
          <div>
            <div className="flex items-center justify-between">
              <span className="px-2.5 py-0.5 rounded-full bg-rose-600 text-white text-[10px] font-black uppercase tracking-wider">
                CODE RED
              </span>
              <span className="text-xs font-mono text-rose-400">880 Hz / 440 Hz</span>
            </div>
            <h3 className="text-sm font-bold text-white mt-2">Kəskin Təcili Vəziyyət</h3>
            <p className="text-xs text-slate-400 mt-1">
              Kəskin Miokard İnfarktı (STEMI), FAST+ İnsult, Massiv qanitirmə və Politravma. İkitonlu avropa tibbi sirenası.
            </p>
          </div>

          <button
            onClick={() => toggleSoundTest("CODE_RED")}
            className={`mt-4 w-full flex items-center justify-center gap-2 py-2 px-3 rounded-xl text-xs font-bold transition ${
              currentPlaying === "CODE_RED"
                ? "bg-rose-600 text-white animate-pulse"
                : "bg-rose-950/60 hover:bg-rose-900/80 text-rose-300 border border-rose-800"
            }`}
          >
            {currentPlaying === "CODE_RED" ? <Square className="w-3.5 h-3.5" /> : <Play className="w-3.5 h-3.5" />}
            <span>{currentPlaying === "CODE_RED" ? "Dayandır" : "Səs Siqnalını Sına (Dual-Tone)"}</span>
          </button>
        </div>

        {/* Code Yellow Card */}
        <div className="bg-slate-900/90 rounded-2xl p-4 border border-amber-500/40 shadow-lg shadow-amber-950/40 flex flex-col justify-between">
          <div>
            <div className="flex items-center justify-between">
              <span className="px-2.5 py-0.5 rounded-full bg-amber-500 text-slate-950 text-[10px] font-black uppercase tracking-wider">
                CODE YELLOW
              </span>
              <span className="text-xs font-mono text-amber-400">520 Hz -&gt; 660 Hz</span>
            </div>
            <h3 className="text-sm font-bold text-white mt-2">Klinik Xəbərdarlıq / Təhlükə</h3>
            <p className="text-xs text-slate-400 mt-1">
              Hipoqlikemiya, Hipertonik kriz, Ağır Aritmiya və ya şöbə daxili pasiyentin pisləşməsi. Yumşaq akkord xəbərdarlığı.
            </p>
          </div>

          <button
            onClick={() => toggleSoundTest("CODE_YELLOW")}
            className={`mt-4 w-full flex items-center justify-center gap-2 py-2 px-3 rounded-xl text-xs font-bold transition ${
              currentPlaying === "CODE_YELLOW"
                ? "bg-amber-600 text-white"
                : "bg-amber-950/60 hover:bg-amber-900/80 text-amber-300 border border-amber-800"
            }`}
          >
            <Play className="w-3.5 h-3.5" />
            <span>Səs Siqnalını Sına (Chime)</span>
          </button>
        </div>
      </div>

      {/* Main Grid: Active Alerts & Manual Trigger Panel */}
      <div className="grid grid-cols-1 lg:grid-cols-12 gap-6">
        {/* Active & Recent Alerts Table (Cols 8) */}
        <div className="lg:col-span-8 bg-slate-900/80 rounded-2xl p-4 border border-slate-800 space-y-4">
          <div className="flex items-center justify-between pb-3 border-b border-slate-800">
            <h3 className="text-sm font-bold text-white uppercase tracking-wider flex items-center gap-2">
              <Bell className="w-4 h-4 text-cyan-400" />
              <span>Aktiv və Son Təcili Həyəcan Jurnalı</span>
            </h3>
            <span className="text-xs text-rose-400 font-mono font-bold">
              {activeAlerts.length} Aktiv Həyəcan
            </span>
          </div>

          {activeAlerts.length > 0 ? (
            <div className="space-y-3">
              {activeAlerts.map((alert) => (
                <div
                  key={alert.id}
                  className={`p-4 rounded-xl border flex flex-col md:flex-row md:items-center justify-between gap-3 ${
                    alert.code_type === "CODE_BLUE"
                      ? "bg-blue-950/40 border-blue-500/70"
                      : "bg-rose-950/40 border-rose-500/70"
                  }`}
                >
                  <div className="space-y-1">
                    <div className="flex items-center gap-2">
                      <span
                        className={`text-[10px] font-black px-2 py-0.5 rounded text-white ${
                          alert.code_type === "CODE_BLUE" ? "bg-blue-600" : "bg-rose-600"
                        }`}
                      >
                        {alert.code_type}
                      </span>
                      <span className="text-xs font-mono text-slate-300">
                        #{alert.id} • {alert.ticket_number || "TƏCİLİ ÇAĞIRIŞ"}
                      </span>
                      <span className="text-xs text-slate-400">• {alert.location}</span>
                    </div>

                    <div className="text-sm font-bold text-white">
                      {alert.patient_name || "Bilinməyən pasiyent"}
                    </div>
                    <div className="text-xs text-rose-300">{alert.reason}</div>
                    <div className="text-[10px] text-slate-500 font-mono">
                      Yaranma: {new Date(alert.created_at).toLocaleString("az-AZ")}
                    </div>
                  </div>

                  <div className="flex items-center gap-2 shrink-0">
                    <button
                      onClick={() => handleAcknowledge(alert.id)}
                      className="px-3 py-2 bg-emerald-600 hover:bg-emerald-500 text-white rounded-lg text-xs font-bold shadow transition"
                    >
                      Təsdiqlə (Acknowledge)
                    </button>
                    <button
                      onClick={() => handleResolve(alert.id)}
                      className="px-3 py-2 bg-slate-800 hover:bg-slate-700 text-slate-300 rounded-lg text-xs font-medium border border-slate-700 transition"
                    >
                      Həll Edildi
                    </button>
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <div className="p-8 text-center bg-slate-950/50 rounded-xl border border-slate-800 text-slate-400 text-xs">
              <CheckCircle2 className="w-8 h-8 text-emerald-500 mx-auto mb-2 opacity-80" />
              Hazırda aktiv təcili həyəcan siqnalı yoxdur. Bütün şöbələr stabildir.
            </div>
          )}

          {/* Recent Alert History */}
          <div className="pt-4 border-t border-slate-800">
            <h4 className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2">
              Son Tarixçə (Arxiv)
            </h4>
            <div className="space-y-1.5 max-h-[160px] overflow-y-auto pr-1">
              {recentAlerts
                .filter((a) => a.status !== "ACTIVE")
                .map((a) => (
                  <div
                    key={a.id}
                    className="p-2 bg-slate-950 rounded-lg border border-slate-800/80 flex items-center justify-between text-xs"
                  >
                    <div className="flex items-center gap-2">
                      <span className="text-[10px] font-mono text-slate-500">#{a.id}</span>
                      <span className="font-semibold text-slate-300">{a.code_type}</span>
                      <span className="text-slate-400 truncate max-w-[200px]">{a.reason}</span>
                    </div>
                    <div className="text-[10px] text-slate-500 font-mono">
                      {new Date(a.created_at).toLocaleTimeString("az-AZ")} • {a.status}
                    </div>
                  </div>
                ))}
            </div>
          </div>

          {/* Real-time Dispatches & Pager Deliveries Log */}
          <div className="pt-4 border-t border-slate-800">
            <h4 className="text-xs font-bold text-cyan-400 uppercase tracking-wider mb-2 flex items-center gap-1.5">
              <span>● Təcili Rabitə & Briqada Çatdırılmaları (Dispatches):</span>
            </h4>
            <div className="space-y-1.5 max-h-[160px] overflow-y-auto pr-1">
              {recentDispatches && recentDispatches.length > 0 ? (
                recentDispatches.map((d: any) => (
                  <div
                    key={d.id}
                    className="p-2 bg-slate-950 rounded-lg border border-slate-800/80 flex items-center justify-between text-xs"
                  >
                    <div className="space-y-0.5">
                      <div className="flex items-center gap-2">
                        <span className="text-[10px] font-bold px-1.5 py-0.2 rounded bg-cyan-950 text-cyan-300 border border-cyan-800">
                          {d.dispatch_type === "AMBULANCE_CALL" ? "BRİQADA ÇAĞIRIŞI" : "HƏKİM PAGER"}
                        </span>
                        <span className="font-semibold text-white">{d.target_team}</span>
                      </div>
                      <div className="text-[11px] text-slate-400">{d.message} • Kanal: {d.channel}</div>
                    </div>
                    <div className="text-right">
                      <span className="text-[10px] font-bold text-emerald-400 bg-emerald-950/60 px-1.5 py-0.5 rounded border border-emerald-800">
                        {d.status}
                      </span>
                      <div className="text-[10px] text-slate-500 font-mono mt-0.5">
                        {new Date(d.created_at).toLocaleTimeString("az-AZ")}
                      </div>
                    </div>
                  </div>
                ))
              ) : (
                <div className="text-[11px] text-slate-500 italic p-2 bg-slate-950/50 rounded-lg">
                  Hələlik göndərilmiş təcili rabitə çağırışı yoxdur.
                </div>
              )}
            </div>
          </div>
        </div>

        {/* Manual Emergency Dispatch Form (Cols 4) */}
        <div className="lg:col-span-4 bg-slate-900/80 rounded-2xl p-4 border border-slate-800 flex flex-col justify-between space-y-4">
          <div>
            <div className="flex items-center gap-2 pb-3 border-b border-slate-800">
              <PlusCircle className="w-4 h-4 text-rose-400" />
              <h3 className="text-sm font-bold text-white uppercase tracking-wider">
                Manual Təcili Çağırış Aç
              </h3>
            </div>

            <form onSubmit={handleManualTrigger} className="mt-3 space-y-3 text-xs">
              <div>
                <label className="text-slate-400 block mb-1 font-medium">Həyəcan Kodu*</label>
                <select
                  value={newCodeType}
                  onChange={(e) =>
                    setNewCodeType(e.target.value as "CODE_BLUE" | "CODE_RED" | "CODE_YELLOW")
                  }
                  className="w-full bg-slate-950 border border-slate-800 rounded-lg px-3 py-2 text-white font-bold focus:border-rose-500 focus:outline-none"
                >
                  <option value="CODE_BLUE">CODE BLUE (Reanimasiya / Asfiksiya)</option>
                  <option value="CODE_RED">CODE RED (Kritik Koronar / İnsult / Şok)</option>
                  <option value="CODE_YELLOW">CODE YELLOW (Klinik Xəbərdarlıq)</option>
                </select>
              </div>

              <div>
                <label className="text-slate-400 block mb-1 font-medium">Məkan / Şöbə / Palata*</label>
                <input
                  type="text"
                  value={newLocation}
                  onChange={(e) => setNewLocation(e.target.value)}
                  className="w-full bg-slate-950 border border-slate-800 rounded-lg px-3 py-2 text-white focus:border-rose-500 focus:outline-none"
                  required
                />
              </div>

              <div>
                <label className="text-slate-400 block mb-1 font-medium">Pasiyentin Adı (əgər məlumdursa)</label>
                <input
                  type="text"
                  value={newPatientName}
                  onChange={(e) => setNewPatientName(e.target.value)}
                  placeholder="Məs: Əhməd Məmmədov"
                  className="w-full bg-slate-950 border border-slate-800 rounded-lg px-3 py-2 text-white focus:border-rose-500 focus:outline-none"
                />
              </div>

              <div>
                <label className="text-slate-400 block mb-1 font-medium">Təcili Səbəb / Vəziyyət*</label>
                <textarea
                  rows={3}
                  value={newReason}
                  onChange={(e) => setNewReason(e.target.value)}
                  className="w-full bg-slate-950 border border-slate-800 rounded-lg px-3 py-2 text-white focus:border-rose-500 focus:outline-none resize-none"
                  required
                />
              </div>

              <button
                type="submit"
                disabled={triggering}
                className="w-full py-3 bg-gradient-to-r from-rose-600 to-red-600 hover:from-rose-500 hover:to-red-500 text-white font-bold rounded-xl shadow-lg shadow-rose-950 transition flex items-center justify-center gap-2"
              >
                <Flame className="w-4 h-4" />
                <span>{triggering ? "Aktivləşdirilir..." : "HƏYƏCAN SİQNALINI AKTİVLƏŞDİR"}</span>
              </button>
            </form>
          </div>

          <div className="p-3 bg-slate-950 rounded-xl border border-slate-800 text-[11px] text-slate-400 space-y-1">
            <span className="font-semibold text-slate-300 block">Qeyd:</span>
            <span>
              Həyəcan aktivləşdirildikdə mərkəzi Web Audio dinamiki işə düşəcək və bütün aktiv terminallara perimeter xəbərdarlıq göndəriləcək.
            </span>
          </div>
        </div>
      </div>
    </div>
  );
}
