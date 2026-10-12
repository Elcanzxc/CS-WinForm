"use client";

import React, { useState, useEffect } from "react";
import {
  BarChart3,
  Search,
  Filter,
  Printer,
  FileText,
  Clock,
  Activity,
  AlertTriangle,
  RefreshCw,
  Eye,
  CheckCircle2,
  X,
  TrendingUp,
  Users,
  Download,
  Check,
} from "lucide-react";
import { PrintableTicketData } from "./PrintReferralTicket";

interface PatientRecord {
  id: number;
  ticket_number: string;
  fin: string;
  full_name: string;
  birth_date: string;
  gender: string;
  phone: string;
  insurance_type: string;
  registered_at: string;
  status: string;
  triage_level: number;
  triage_color: string;
  assigned_department: string;
  assigned_cabinet: string;
  assigned_doctor: string;
  sys?: number;
  dia?: number;
  pulse?: number;
  spo2?: number;
  temp?: number;
  glucose?: number;
  vas_pain?: number;
  symptoms_json?: string;
  triage_wait_time?: string;
  suspected_diagnosis?: string;
  icd10_code?: string;
  emergency_exams_json?: string;
  clinical_notes?: string;
}

interface AuditStatisticsProps {
  onPrintPatient: (data: PrintableTicketData) => void;
}

export default function AuditStatistics({ onPrintPatient }: AuditStatisticsProps) {
  const [patients, setPatients] = useState<PatientRecord[]>([]);
  const [stats, setStats] = useState<any>(null);
  const [auditLogs, setAuditLogs] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState("");
  const [levelFilter, setLevelFilter] = useState("ALL");
  const [statusFilter, setStatusFilter] = useState("ALL");
  const [selectedPatient, setSelectedPatient] = useState<PatientRecord | null>(null);

  const fetchData = async () => {
    try {
      setLoading(true);
      const params = new URLSearchParams();
      if (search) params.append("search", search);
      if (levelFilter !== "ALL") params.append("level", levelFilter);
      if (statusFilter !== "ALL") params.append("status", statusFilter);

      const [pRes, sRes] = await Promise.all([
        fetch(`/api/patients?${params.toString()}`),
        fetch("/api/stats"),
      ]);

      const pJson = await pRes.json();
      const sJson = await sRes.json();

      if (pJson.success) setPatients(pJson.data);
      if (sJson.success) {
        setStats(sJson.stats);
        setAuditLogs(sJson.auditLogs);
      }
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, [levelFilter, statusFilter]);

  const handleSearchSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    fetchData();
  };

  const getTriageBadge = (level: number, color: string) => {
    switch (level) {
      case 1:
        return "bg-rose-500/20 text-rose-300 border-rose-500/40";
      case 2:
        return "bg-orange-500/20 text-orange-300 border-orange-500/40";
      case 3:
        return "bg-amber-500/20 text-amber-300 border-amber-500/40";
      case 4:
        return "bg-emerald-500/20 text-emerald-300 border-emerald-500/40";
      default:
        return "bg-sky-500/20 text-sky-300 border-sky-500/40";
    }
  };

  const handlePrintClick = (p: PatientRecord) => {
    let exams: string[] = [];
    try {
      exams = p.emergency_exams_json ? JSON.parse(p.emergency_exams_json) : [];
    } catch {}

    const printData: PrintableTicketData = {
      ticketNumber: p.ticket_number,
      fin: p.fin,
      fullName: p.full_name,
      birthDate: p.birth_date,
      gender: p.gender,
      phone: p.phone,
      insuranceType: p.insurance_type,
      registeredAt: new Date(p.registered_at).toLocaleString("az-AZ"),
      triageLevel: p.triage_level,
      triageColor: p.triage_color,
      triageWaitTime: p.triage_wait_time || "Standart",
      assignedDepartment: p.assigned_department,
      assignedCabinet: p.assigned_cabinet,
      assignedDoctor: p.assigned_doctor,
      suspectedDiagnosis: p.suspected_diagnosis || "Təyin edilməyib",
      icd10Code: p.icd10_code || "Z00.0",
      emergencyExams: exams,
      vitals: {
        sys: p.sys ?? 120,
        dia: p.dia ?? 80,
        pulse: p.pulse ?? 75,
        spo2: p.spo2 ?? 98,
        temp: p.temp ?? 36.6,
        glucose: p.glucose ?? 5.0,
        vasPain: p.vas_pain ?? 0,
      },
      notes: p.clinical_notes,
    };

    onPrintPatient(printData);
  };

  // Export shift report as CSV
  const handleExportCSV = () => {
    if (patients.length === 0) return;

    const headers = [
      "Talon",
      "FIN",
      "Ad Soyad",
      "Doğum Tarixi",
      "Cins",
      "Telefon",
      "Sığorta",
      "MTS Səviyyə",
      "MTS Rəng",
      "İlkin Diaqnoz",
      "XBT Kodu",
      "Şöbə",
      "Kabinet",
      "Status",
      "Qeydiyyat Tarixi",
    ];

    const rows = patients.map((p) => [
      p.ticket_number,
      p.fin,
      `"${p.full_name}"`,
      p.birth_date,
      p.gender,
      `"${p.phone}"`,
      `"${p.insurance_type}"`,
      p.triage_level,
      p.triage_color,
      `"${p.suspected_diagnosis || ""}"`,
      p.icd10_code || "",
      `"${p.assigned_department}"`,
      p.assigned_cabinet,
      `"${p.status}"`,
      p.registered_at,
    ]);

    const csvContent =
      "data:text/csv;charset=utf-8,\uFEFF" +
      [headers.join(","), ...rows.map((r) => r.join(","))].join("\n");

    const encodedUri = encodeURI(csvContent);
    const link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute(
      "download",
      `MedTriage_Novbe_Hesabati_${new Date().toISOString().split("T")[0]}.csv`
    );
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  return (
    <div className="space-y-6">
      {/* Top Analytical KPI Widgets */}
      <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
        <div className="bg-slate-900/90 rounded-2xl p-4 border border-slate-800 shadow-lg">
          <div className="text-slate-400 text-xs flex items-center justify-between">
            <span>Toplam Registrasiya</span>
            <Users className="w-4 h-4 text-cyan-400" />
          </div>
          <div className="text-3xl font-black text-white mt-2 font-mono">
            {stats?.totalPatients ?? patients.length}
          </div>
          <div className="text-[11px] text-slate-500 mt-1">Cari 24 saatlıq növbədə</div>
        </div>

        <div className="bg-slate-900/90 rounded-2xl p-4 border border-rose-500/40 shadow-lg shadow-rose-950/20">
          <div className="text-slate-400 text-xs flex items-center justify-between">
            <span>Kritik Hallar (L1 Qırmızı / L2 Narıncı)</span>
            <AlertTriangle className="w-4 h-4 text-rose-400" />
          </div>
          <div className="text-3xl font-black text-rose-400 mt-2 font-mono">
            {(stats?.triageDistribution?.level1Red || 0) + (stats?.triageDistribution?.level2Orange || 0)}
          </div>
          <div className="text-[11px] text-slate-500 mt-1">Dərhal reanimasion müdaxilə</div>
        </div>

        <div className="bg-slate-900/90 rounded-2xl p-4 border border-slate-800 shadow-lg">
          <div className="text-slate-400 text-xs flex items-center justify-between">
            <span>Aktiv Həyəcanlar (Alerts)</span>
            <Activity className="w-4 h-4 text-amber-400" />
          </div>
          <div className="text-3xl font-black text-amber-400 mt-2 font-mono">
            {stats?.activeEmergencies ?? 0}
          </div>
          <div className="text-[11px] text-slate-500 mt-1">Həkim təsdiqi gözləyən</div>
        </div>

        <div className="bg-slate-900/90 rounded-2xl p-4 border border-slate-800 shadow-lg">
          <div className="text-slate-400 text-xs flex items-center justify-between">
            <span>MTS İcra Faizi</span>
            <Clock className="w-4 h-4 text-emerald-400" />
          </div>
          <div className="text-3xl font-black text-emerald-400 mt-2 font-mono">
            98.4%
          </div>
          <div className="text-[11px] text-slate-500 mt-1">Protokol vaxtına tam riayət</div>
        </div>
      </div>

      {/* MTS Distribution Bar */}
      {stats?.triageDistribution && (
        <div className="bg-slate-900/90 rounded-2xl p-4 border border-slate-800 space-y-2">
          <div className="flex items-center justify-between text-xs">
            <span className="font-bold text-slate-200">
              Manchester Triaq Sistemi (MTS) Risk Səviyyələri Paylanması:
            </span>
            <span className="text-slate-400 font-mono">Cəmi: {stats.totalPatients} pasiyent</span>
          </div>

          <div className="h-3.5 rounded-full bg-slate-950 flex overflow-hidden border border-slate-800">
            <div
              style={{
                width: `${((stats.triageDistribution.level1Red || 0) / (stats.totalPatients || 1)) * 100}%`,
              }}
              className="bg-rose-600 transition-all"
              title={`L1 Qırmızı: ${stats.triageDistribution.level1Red}`}
            />
            <div
              style={{
                width: `${((stats.triageDistribution.level2Orange || 0) / (stats.totalPatients || 1)) * 100}%`,
              }}
              className="bg-orange-500 transition-all"
              title={`L2 Narıncı: ${stats.triageDistribution.level2Orange}`}
            />
            <div
              style={{
                width: `${((stats.triageDistribution.level3Yellow || 0) / (stats.totalPatients || 1)) * 100}%`,
              }}
              className="bg-amber-400 transition-all"
              title={`L3 Sarı: ${stats.triageDistribution.level3Yellow}`}
            />
            <div
              style={{
                width: `${((stats.triageDistribution.level4Green || 0) / (stats.totalPatients || 1)) * 100}%`,
              }}
              className="bg-emerald-500 transition-all"
              title={`L4 Yaşıl: ${stats.triageDistribution.level4Green}`}
            />
            <div
              style={{
                width: `${((stats.triageDistribution.level5Blue || 0) / (stats.totalPatients || 1)) * 100}%`,
              }}
              className="bg-sky-500 transition-all"
              title={`L5 Mavi: ${stats.triageDistribution.level5Blue}`}
            />
          </div>

          <div className="flex flex-wrap items-center justify-between text-[11px] text-slate-400 pt-1 font-mono">
            <span className="flex items-center gap-1.5">
              <span className="w-2.5 h-2.5 rounded-full bg-rose-600" />
              L1 Qırmızı (Dərhal): {stats.triageDistribution.level1Red || 0}
            </span>
            <span className="flex items-center gap-1.5">
              <span className="w-2.5 h-2.5 rounded-full bg-orange-500" />
              L2 Narıncı (10 dəq): {stats.triageDistribution.level2Orange || 0}
            </span>
            <span className="flex items-center gap-1.5">
              <span className="w-2.5 h-2.5 rounded-full bg-amber-400" />
              L3 Sarı (60 dəq): {stats.triageDistribution.level3Yellow || 0}
            </span>
            <span className="flex items-center gap-1.5">
              <span className="w-2.5 h-2.5 rounded-full bg-emerald-500" />
              L4 Yaşıl (120 dəq): {stats.triageDistribution.level4Green || 0}
            </span>
            <span className="flex items-center gap-1.5">
              <span className="w-2.5 h-2.5 rounded-full bg-sky-500" />
              L5 Mavi (Planlı): {stats.triageDistribution.level5Blue || 0}
            </span>
          </div>
        </div>
      )}

      {/* Patients Filter & Search Bar */}
      <div className="bg-slate-900/90 rounded-2xl p-4 border border-slate-800 space-y-4">
        <div className="flex flex-wrap items-center justify-between gap-3">
          <form onSubmit={handleSearchSubmit} className="flex items-center gap-2 flex-1 min-w-[260px]">
            <div className="relative w-full">
              <Search className="w-4 h-4 absolute left-3 top-2.5 text-slate-500" />
              <input
                type="text"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                placeholder="FİN, Ad/Soyad və ya Talon ilə axtarış..."
                className="w-full bg-slate-950 border border-slate-800 rounded-lg pl-9 pr-3 py-2 text-xs text-white focus:outline-none focus:border-cyan-500"
              />
            </div>
            <button
              type="submit"
              className="px-4 py-2 bg-cyan-600 hover:bg-cyan-500 text-white rounded-lg text-xs font-bold transition shrink-0"
            >
              Axtar
            </button>
          </form>

          <div className="flex flex-wrap items-center gap-2 text-xs">
            <select
              value={levelFilter}
              onChange={(e) => setLevelFilter(e.target.value)}
              className="bg-slate-950 border border-slate-800 rounded-lg px-2.5 py-2 text-slate-300 focus:outline-none"
            >
              <option value="ALL">Bütün Səviyyələr (L1-L5)</option>
              <option value="1">Level 1 - Qırmızı (Reanimasiya)</option>
              <option value="2">Level 2 - Narıncı (10 dəq)</option>
              <option value="3">Level 3 - Sarı (60 dəq)</option>
              <option value="4">Level 4 - Yaşıl (120 dəq)</option>
              <option value="5">Level 5 - Mavi (Planlı)</option>
            </select>

            <select
              value={statusFilter}
              onChange={(e) => setStatusFilter(e.target.value)}
              className="bg-slate-950 border border-slate-800 rounded-lg px-2.5 py-2 text-slate-300 focus:outline-none"
            >
              <option value="ALL">Bütün Statuslar</option>
              <option value="Gözləyir">Gözləyir</option>
              <option value="Həkim qəbulunda">Həkim qəbulunda</option>
              <option value="Təxirəsalınmaz Reanimasiya">Təxirəsalınmaz Reanimasiya</option>
              <option value="Tamamlandı">Tamamlandı</option>
            </select>

            {/* CSV Export Button */}
            <button
              type="button"
              onClick={handleExportCSV}
              className="flex items-center gap-1.5 px-3 py-2 bg-slate-800 hover:bg-slate-700 text-slate-200 rounded-lg font-bold border border-slate-700 transition"
              title="CSV / Excel formatında yüklə"
            >
              <Download className="w-3.5 h-3.5 text-cyan-400" />
              <span>CSV İxrac</span>
            </button>

            <button
              type="button"
              onClick={fetchData}
              className="p-2 bg-slate-800 hover:bg-slate-700 text-slate-300 rounded-lg transition"
              title="Yenilə"
            >
              <RefreshCw className={`w-4 h-4 ${loading ? "animate-spin text-cyan-400" : ""}`} />
            </button>
          </div>
        </div>

        {/* Patients Table */}
        <div className="overflow-x-auto rounded-xl border border-slate-800">
          <table className="w-full text-left text-xs text-slate-300">
            <thead className="bg-slate-950 text-slate-400 uppercase font-semibold text-[10px] border-b border-slate-800">
              <tr>
                <th className="p-3">Talon</th>
                <th className="p-3">Pasiyent (Ad/Soyad & FİN)</th>
                <th className="p-3">MTS Səviyyəsi</th>
                <th className="p-3">İlkin Diaqnoz</th>
                <th className="p-3">Təyin Edilən Şöbə</th>
                <th className="p-3">Status</th>
                <th className="p-3">Qeydiyyat</th>
                <th className="p-3 text-right">Əməliyyat</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-800/60 font-sans">
              {patients.length > 0 ? (
                patients.map((p) => (
                  <tr key={p.id} className="hover:bg-slate-800/40 transition">
                    <td className="p-3 font-mono font-bold text-cyan-400">{p.ticket_number}</td>
                    <td className="p-3">
                      <div className="font-bold text-white">{p.full_name}</div>
                      <div className="text-[10px] text-slate-500 font-mono">
                        FİN: {p.fin} • {p.gender}
                      </div>
                    </td>
                    <td className="p-3">
                      <span
                        className={`inline-block px-2.5 py-0.5 rounded text-[10px] font-bold border ${getTriageBadge(
                          p.triage_level,
                          p.triage_color
                        )}`}
                      >
                        L{p.triage_level}: {p.triage_color}
                      </span>
                    </td>
                    <td className="p-3">
                      <div className="font-medium text-slate-200 truncate max-w-[200px]">
                        {p.suspected_diagnosis || "Diaqnoz daxil edilməyib"}
                      </div>
                      <div className="text-[10px] text-slate-500 font-mono">
                        XBT: {p.icd10_code || "-"}
                      </div>
                    </td>
                    <td className="p-3">
                      <div className="text-slate-300">{p.assigned_department}</div>
                      <div className="text-[10px] text-cyan-400 font-mono">
                        {p.assigned_cabinet} ({p.assigned_doctor})
                      </div>
                    </td>
                    <td className="p-3">
                      <span className="text-[10px] px-2.5 py-0.5 rounded bg-slate-800 text-slate-300 font-medium">
                        {p.status}
                      </span>
                    </td>
                    <td className="p-3 text-[10px] text-slate-500 font-mono">
                      {new Date(p.registered_at).toLocaleTimeString("az-AZ")}
                    </td>
                    <td className="p-3 text-right">
                      <div className="flex items-center justify-end gap-1">
                        <button
                          onClick={() => setSelectedPatient(p)}
                          className="p-1.5 hover:bg-slate-800 text-slate-400 hover:text-cyan-400 rounded transition"
                          title="Ətraflı Bax"
                        >
                          <Eye className="w-4 h-4" />
                        </button>
                        <button
                          onClick={() => handlePrintClick(p)}
                          className="p-1.5 hover:bg-slate-800 text-slate-400 hover:text-white rounded transition"
                          title="Talonu Çap Et"
                        >
                          <Printer className="w-4 h-4" />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={8} className="p-8 text-center text-slate-500">
                    Axtarış parametrlərinə uyğun qeydiyyat tapılmadı.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>

      {/* System Audit Trail Logs Table */}
      <div className="bg-slate-900/90 rounded-2xl p-4 border border-slate-800 space-y-3">
        <h3 className="text-xs font-bold text-slate-400 uppercase tracking-wider flex items-center gap-2">
          <FileText className="w-4 h-4 text-cyan-400" />
          <span>Sistem Təhlükəsizlik və Audit Jurnalı (Audit Trail)</span>
        </h3>
        <div className="space-y-1.5 max-h-[160px] overflow-y-auto pr-1">
          {auditLogs.map((log) => (
            <div
              key={log.id}
              className="p-2 bg-slate-950 rounded-lg border border-slate-800/80 flex items-center justify-between text-xs"
            >
              <div className="flex items-center gap-2">
                <span className="text-[10px] font-mono text-cyan-400 font-bold">[{log.action_type}]</span>
                <span className="text-slate-300 truncate max-w-[500px]">{log.description}</span>
              </div>
              <div className="text-[10px] text-slate-500 font-mono">
                {log.user_role} • {new Date(log.created_at).toLocaleTimeString("az-AZ")}
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Patient Detail Modal */}
      {selectedPatient && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/80 backdrop-blur-sm">
          <div className="w-full max-w-xl bg-slate-900 border border-slate-800 rounded-2xl p-6 shadow-2xl text-xs space-y-4">
            <div className="flex items-center justify-between border-b border-slate-800 pb-3">
              <div>
                <span className="font-mono text-cyan-400 font-bold text-base">
                  {selectedPatient.ticket_number}
                </span>
                <h3 className="text-base font-bold text-white mt-0.5">{selectedPatient.full_name}</h3>
              </div>
              <button
                onClick={() => setSelectedPatient(null)}
                className="p-1 text-slate-400 hover:text-white rounded"
              >
                <X className="w-5 h-5" />
              </button>
            </div>

            <div className="grid grid-cols-2 gap-3 bg-slate-950 p-3 rounded-xl border border-slate-800">
              <div>
                <span className="text-slate-500 block">FİN Kod:</span>
                <span className="font-mono font-bold text-white">{selectedPatient.fin}</span>
              </div>
              <div>
                <span className="text-slate-500 block">Doğum Tarixi:</span>
                <span className="font-semibold text-white">{selectedPatient.birth_date}</span>
              </div>
              <div>
                <span className="text-slate-500 block">Telefon:</span>
                <span className="text-white">{selectedPatient.phone}</span>
              </div>
              <div>
                <span className="text-slate-500 block">Sığorta:</span>
                <span className="text-cyan-400 font-medium">{selectedPatient.insurance_type}</span>
              </div>
            </div>

            {/* Vitals */}
            <div>
              <span className="font-bold text-slate-300 block mb-1">Vital Göstəricilər:</span>
              <div className="grid grid-cols-3 gap-2 bg-slate-950 p-2.5 rounded-xl border border-slate-800 text-center">
                <div>
                  <span className="text-slate-500 text-[10px]">Təzyiq</span>
                  <div className="font-mono font-bold text-white">
                    {selectedPatient.sys}/{selectedPatient.dia} mmHg
                  </div>
                </div>
                <div>
                  <span className="text-slate-500 text-[10px]">Nəbz</span>
                  <div className="font-mono font-bold text-white">{selectedPatient.pulse} BPM</div>
                </div>
                <div>
                  <span className="text-slate-500 text-[10px]">SpO2</span>
                  <div className="font-mono font-bold text-white">{selectedPatient.spo2}%</div>
                </div>
                <div>
                  <span className="text-slate-500 text-[10px]">Hərarət</span>
                  <div className="font-mono font-bold text-white">{selectedPatient.temp}°C</div>
                </div>
                <div>
                  <span className="text-slate-500 text-[10px]">Qlükoza</span>
                  <div className="font-mono font-bold text-white">{selectedPatient.glucose} mmol/L</div>
                </div>
                <div>
                  <span className="text-slate-500 text-[10px]">VAS Ağrı</span>
                  <div className="font-mono font-bold text-white">{selectedPatient.vas_pain}/10</div>
                </div>
              </div>
            </div>

            {/* Diagnosis & Routing */}
            <div className="bg-slate-950 p-3 rounded-xl border border-slate-800 space-y-1.5">
              <div className="text-slate-400">
                İlkin Diaqnoz:{" "}
                <span className="text-white font-bold">{selectedPatient.suspected_diagnosis}</span>
              </div>
              <div className="text-slate-400">
                XBT-10 Kodu: <span className="text-cyan-400 font-mono">{selectedPatient.icd10_code}</span>
              </div>
              <div className="text-slate-400">
                Şöbə & Kabinet:{" "}
                <span className="text-white">
                  {selectedPatient.assigned_department} • {selectedPatient.assigned_cabinet}
                </span>
              </div>
              {selectedPatient.clinical_notes && (
                <div className="text-slate-400 pt-1 border-t border-slate-800">
                  Qeyd: <span className="text-slate-300">{selectedPatient.clinical_notes}</span>
                </div>
              )}
            </div>

            <div className="flex justify-end gap-2 pt-2 border-t border-slate-800">
              <button
                onClick={() => {
                  handlePrintClick(selectedPatient);
                  setSelectedPatient(null);
                }}
                className="flex items-center gap-1.5 px-4 py-2 bg-cyan-600 hover:bg-cyan-500 text-white rounded-lg font-bold shadow"
              >
                <Printer className="w-4 h-4" />
                <span>Triaq Talonu Çap Et</span>
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
