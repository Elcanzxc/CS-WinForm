"use client";

import React from "react";
import { Printer, X, ShieldCheck, QrCode } from "lucide-react";

export interface PrintableTicketData {
  ticketNumber: string;
  fin: string;
  fullName: string;
  birthDate: string;
  gender: string;
  phone: string;
  insuranceType: string;
  registeredAt?: string;
  triageLevel: number;
  triageColor: string;
  triageWaitTime: string;
  assignedDepartment: string;
  assignedCabinet: string;
  assignedDoctor: string;
  suspectedDiagnosis: string;
  icd10Code: string;
  emergencyExams: string[];
  vitals: {
    sys: number;
    dia: number;
    pulse: number;
    spo2: number;
    temp: number;
    glucose: number;
    vasPain: number;
  };
  notes?: string;
}

interface PrintReferralTicketProps {
  data: PrintableTicketData | null;
  onClose: () => void;
}

export default function PrintReferralTicket({
  data,
  onClose,
}: PrintReferralTicketProps) {
  if (!data) return null;

  const handlePrint = () => {
    window.print();
  };

  const getTriageBadgeStyle = (level: number) => {
    switch (level) {
      case 1:
        return "bg-rose-100 text-rose-950 border-rose-600";
      case 2:
        return "bg-orange-100 text-orange-950 border-orange-600";
      case 3:
        return "bg-amber-100 text-amber-950 border-amber-600";
      case 4:
        return "bg-emerald-100 text-emerald-950 border-emerald-600";
      default:
        return "bg-sky-100 text-sky-950 border-sky-600";
    }
  };

  // Shock index calculation
  const shockIndex = data.vitals.sys > 0 ? (data.vitals.pulse / data.vitals.sys).toFixed(2) : "0.00";

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/80 backdrop-blur-sm overflow-y-auto">
      <div className="relative w-full max-w-2xl bg-white text-slate-900 rounded-2xl shadow-2xl p-6 my-8 border border-slate-200">
        {/* Top UI Action Bar (hidden when printing) */}
        <div className="no-print flex items-center justify-between pb-4 mb-4 border-b border-slate-200">
          <div className="flex items-center gap-2 text-slate-700">
            <Printer className="w-5 h-5 text-cyan-600" />
            <span className="font-bold text-sm">
              Klinik Triaq İstiqamətləndirmə Vərəqi və Növbə Talonu (Çap)
            </span>
          </div>
          <div className="flex items-center gap-2">
            <button
              onClick={handlePrint}
              className="flex items-center gap-2 px-4 py-2 bg-cyan-600 hover:bg-cyan-500 text-white rounded-lg text-xs font-bold shadow transition"
            >
              <Printer className="w-4 h-4" />
              <span>Çap Et (Ctrl + P)</span>
            </button>
            <button
              onClick={onClose}
              className="p-2 text-slate-400 hover:text-slate-700 hover:bg-slate-100 rounded-lg transition"
            >
              <X className="w-5 h-5" />
            </button>
          </div>
        </div>

        {/* Printable Paper Document */}
        <div id="printable-ticket" className="font-sans text-xs text-slate-900 space-y-4">
          {/* Header */}
          <div className="border-b-2 border-slate-900 pb-3 text-center relative">
            <div className="text-[10px] uppercase font-bold tracking-widest text-slate-600">
              Azərbaycan Respublikası Səhiyyə Nazirliyi
            </div>
            <div className="text-base font-black tracking-wide text-slate-950 uppercase mt-0.5">
              Bakı Klinik Təcili və Təxirəsalınmaz Tibbi Mərkəzi
            </div>
            <div className="text-[11px] text-slate-600 font-semibold">
              Rəsmi Triaq Qəbulu, İlkin Diaqnostika və Həkim İstiqamətləndirmə Blankı
            </div>
          </div>

          {/* Ticket Number & Triage Level Stamp */}
          <div className="flex items-center justify-between bg-slate-50 p-3 rounded-lg border border-slate-300">
            <div>
              <div className="text-[10px] text-slate-500 uppercase font-semibold">Növbə / Talon Nömrəsi</div>
              <div className="text-4xl font-black font-mono tracking-tight text-slate-950">
                {data.ticketNumber}
              </div>
              <div className="text-[10px] text-slate-600 mt-1">
                Qeydiyyat: {data.registeredAt || new Date().toLocaleString("az-AZ")}
              </div>
            </div>

            {/* Simulated Vector Barcode */}
            <div className="text-center px-4">
              <svg className="w-36 h-10 mx-auto" viewBox="0 0 140 40">
                <rect x="0" y="0" width="3" height="35" fill="#000" />
                <rect x="5" y="0" width="1" height="35" fill="#000" />
                <rect x="8" y="0" width="4" height="35" fill="#000" />
                <rect x="14" y="0" width="2" height="35" fill="#000" />
                <rect x="18" y="0" width="1" height="35" fill="#000" />
                <rect x="21" y="0" width="5" height="35" fill="#000" />
                <rect x="28" y="0" width="2" height="35" fill="#000" />
                <rect x="32" y="0" width="3" height="35" fill="#000" />
                <rect x="37" y="0" width="1" height="35" fill="#000" />
                <rect x="40" y="0" width="4" height="35" fill="#000" />
                <rect x="46" y="0" width="2" height="35" fill="#000" />
                <rect x="50" y="0" width="5" height="35" fill="#000" />
                <rect x="57" y="0" width="2" height="35" fill="#000" />
                <rect x="61" y="0" width="1" height="35" fill="#000" />
                <rect x="64" y="0" width="4" height="35" fill="#000" />
                <rect x="70" y="0" width="2" height="35" fill="#000" />
                <rect x="74" y="0" width="3" height="35" fill="#000" />
                <rect x="79" y="0" width="1" height="35" fill="#000" />
                <rect x="82" y="0" width="4" height="35" fill="#000" />
                <rect x="88" y="0" width="2" height="35" fill="#000" />
                <rect x="92" y="0" width="5" height="35" fill="#000" />
                <rect x="99" y="0" width="2" height="35" fill="#000" />
                <rect x="103" y="0" width="1" height="35" fill="#000" />
                <rect x="106" y="0" width="4" height="35" fill="#000" />
                <rect x="112" y="0" width="3" height="35" fill="#000" />
                <rect x="117" y="0" width="1" height="35" fill="#000" />
                <rect x="120" y="0" width="5" height="35" fill="#000" />
                <rect x="127" y="0" width="2" height="35" fill="#000" />
                <rect x="131" y="0" width="3" height="35" fill="#000" />
                <rect x="136" y="0" width="2" height="35" fill="#000" />
              </svg>
              <span className="font-mono text-[9px] text-slate-700 font-bold block mt-0.5">
                *{data.ticketNumber}-{data.fin}*
              </span>
            </div>

            <div className="text-right">
              <div className="text-[10px] text-slate-500 uppercase font-semibold">Manchester Triaqı (MTS)</div>
              <div
                className={`inline-block px-3 py-1.5 rounded-md border font-black text-xs uppercase mt-1 ${getTriageBadgeStyle(
                  data.triageLevel
                )}`}
              >
                Səviyyə {data.triageLevel}: {data.triageColor}
              </div>
              <div className="text-[10px] text-slate-700 mt-1 font-bold">
                Maksimal Gözləmə: {data.triageWaitTime}
              </div>
            </div>
          </div>

          {/* Patient Details Table */}
          <div>
            <div className="text-[11px] font-bold uppercase tracking-wider text-slate-700 mb-1 border-b pb-0.5">
              1. Pasiyent Məlumatları
            </div>
            <div className="grid grid-cols-3 gap-2 bg-slate-50 p-2.5 rounded border border-slate-200 text-[11px]">
              <div>
                <span className="text-slate-500 block text-[10px]">Adı, Soyadı:</span>
                <span className="font-bold text-slate-900">{data.fullName}</span>
              </div>
              <div>
                <span className="text-slate-500 block text-[10px]">FİN Kod:</span>
                <span className="font-mono font-bold text-slate-900">{data.fin}</span>
              </div>
              <div>
                <span className="text-slate-500 block text-[10px]">Doğum Tarixi:</span>
                <span className="font-semibold text-slate-900">{data.birthDate}</span>
              </div>
              <div>
                <span className="text-slate-500 block text-[10px]">Cinsiyyət:</span>
                <span className="font-semibold text-slate-900">{data.gender}</span>
              </div>
              <div>
                <span className="text-slate-500 block text-[10px]">Əlaqə Telefonu:</span>
                <span className="font-semibold text-slate-900">{data.phone}</span>
              </div>
              <div>
                <span className="text-slate-500 block text-[10px]">Sığorta Təminatı:</span>
                <span className="font-bold text-cyan-800">{data.insuranceType}</span>
              </div>
            </div>
          </div>

          {/* Vitals & Clinical Indices Table */}
          <div>
            <div className="text-[11px] font-bold uppercase tracking-wider text-slate-700 mb-1 border-b pb-0.5 flex items-center justify-between">
              <span>2. Qəbul Anında Vital Göstəricilər və Şok İndeksi</span>
              <span className="font-mono text-[10px] text-slate-600 font-bold">
                Şok İndeksi: {shockIndex} (Nəbz/SYS)
              </span>
            </div>
            <table className="w-full text-center border border-slate-300 text-[10px]">
              <thead className="bg-slate-100 border-b border-slate-300 font-bold text-slate-700">
                <tr>
                  <th className="p-1.5 border-r border-slate-300">Təzyiq (SYS/DIA)</th>
                  <th className="p-1.5 border-r border-slate-300">Nəbz (BPM)</th>
                  <th className="p-1.5 border-r border-slate-300">SpO2 (%)</th>
                  <th className="p-1.5 border-r border-slate-300">Hərarət (°C)</th>
                  <th className="p-1.5 border-r border-slate-300">Qlükoza (mmol/L)</th>
                  <th className="p-1.5">Ağrı (VAS 0-10)</th>
                </tr>
              </thead>
              <tbody className="font-bold text-slate-900">
                <tr>
                  <td className="p-1.5 border-r border-slate-300 font-mono">
                    {data.vitals.sys}/{data.vitals.dia} mmHg
                  </td>
                  <td className="p-1.5 border-r border-slate-300 font-mono">{data.vitals.pulse} bpm</td>
                  <td className="p-1.5 border-r border-slate-300 font-mono">{data.vitals.spo2}%</td>
                  <td className="p-1.5 border-r border-slate-300 font-mono">{data.vitals.temp} °C</td>
                  <td className="p-1.5 border-r border-slate-300 font-mono">{data.vitals.glucose} mmol/L</td>
                  <td className="p-1.5 font-mono">{data.vitals.vasPain} / 10</td>
                </tr>
              </tbody>
            </table>
          </div>

          {/* Clinical Direction & CDSS Conclusion */}
          <div>
            <div className="text-[11px] font-bold uppercase tracking-wider text-slate-700 mb-1 border-b pb-0.5">
              3. Triaq Qərarı və Yönləndirmə
            </div>
            <div className="space-y-1.5 bg-slate-50 p-2.5 rounded border border-slate-200 text-[11px]">
              <div className="flex justify-between">
                <span className="text-slate-500">Təyin Edilən Şöbə:</span>
                <span className="font-bold text-slate-950">{data.assignedDepartment}</span>
              </div>
              <div className="flex justify-between">
                <span className="text-slate-500">Kabinet və Mütəxəssis:</span>
                <span className="font-bold text-cyan-900">
                  Kabinet: {data.assignedCabinet} • {data.assignedDoctor}
                </span>
              </div>
              <div className="flex justify-between border-t border-slate-200 pt-1">
                <span className="text-slate-500">İlkin Diaqnoz Şübhəsi:</span>
                <span className="font-bold text-slate-900">
                  {data.suspectedDiagnosis} (XBT-10: {data.icd10Code})
                </span>
              </div>
              {data.notes && (
                <div className="border-t border-slate-200 pt-1 text-[10px] text-slate-700">
                  <span className="font-bold">Klinik Qeydlər və Allergiya:</span> {data.notes}
                </div>
              )}
            </div>
          </div>

          {/* Emergency STAT Exams */}
          {data.emergencyExams && data.emergencyExams.length > 0 && (
            <div>
              <div className="text-[11px] font-bold uppercase tracking-wider text-slate-700 mb-1 border-b pb-0.5">
                4. Təcili Tələb Olunan Müayinələr (STAT Protocols)
              </div>
              <ul className="list-disc list-inside bg-slate-50 p-2 rounded border border-slate-200 text-[10px] space-y-0.5 text-slate-800">
                {data.emergencyExams.map((ex, idx) => (
                  <li key={idx} className="font-medium">
                    {ex}
                  </li>
                ))}
              </ul>
            </div>
          )}

          {/* Footer & Signature */}
          <div className="border-t-2 border-slate-900 pt-3 flex items-center justify-between text-[10px] text-slate-600">
            <div>
              <div>Operator / Triaq Tibb Bacısı: ______________________</div>
              <div className="text-[9px] text-slate-500 mt-0.5">
                Elektron generasiya tarixi: {new Date().toLocaleString("az-AZ")}
              </div>
            </div>
            <div className="text-right">
              <div>Növbətçi Həkim İmzası: ______________________</div>
              <div className="font-mono text-[9px] text-slate-400 mt-0.5">MÖHÜR ÜÇÜN YER</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
