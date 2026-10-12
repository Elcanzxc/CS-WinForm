"use client";

import React, { useState, useEffect } from "react";
import Header from "@/components/Header";
import ReceptionTriage from "@/components/ReceptionTriage";
import EmergencyTerminal from "@/components/EmergencyTerminal";
import CabinetDispatch from "@/components/CabinetDispatch";
import AuditStatistics from "@/components/AuditStatistics";
import EmergencyOverlay, { EmergencyAlertItem } from "@/components/EmergencyOverlay";
import PrintReferralTicket, { PrintableTicketData } from "@/components/PrintReferralTicket";
import QueueMonitorModal from "@/components/QueueMonitorModal";
import { playEmergencyAlert } from "@/lib/audioAlerts";

export default function MedTriageDashboard() {
  const [activeTab, setActiveTab] = useState<string>("reception");
  const [alerts, setAlerts] = useState<EmergencyAlertItem[]>([]);
  const [printableData, setPrintableData] = useState<PrintableTicketData | null>(null);
  const [isQueueMonitorOpen, setIsQueueMonitorOpen] = useState(false);

  // Poll active alerts
  const fetchAlerts = async () => {
    try {
      const res = await fetch("/api/alerts");
      const json = await res.json();
      if (json.success) {
        setAlerts(json.activeAlerts);
      }
    } catch (err) {
      console.error("Alerts fetch error:", err);
    }
  };

  useEffect(() => {
    fetchAlerts();
    const interval = setInterval(fetchAlerts, 4000);
    return () => clearInterval(interval);
  }, []);

  const handleTriggerPanic = async (code: "CODE_BLUE" | "CODE_RED") => {
    try {
      const location = code === "CODE_BLUE" ? "Şok Zalı / Reanimasiya Bloku" : "Klinik Qəbul Terminalı";
      const reason =
        code === "CODE_BLUE"
          ? "Terminaldan təcili reanimasiya panic çağırışı"
          : "Terminaldan kəskin təcili vəziyyət (Code Red) çağırışı";

      await fetch("/api/alerts", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          codeType: code,
          location,
          reason,
          patientName: "TƏCİLİ PANİC ÇAĞIRIŞ",
        }),
      });

      playEmergencyAlert(code);
      await fetchAlerts();
    } catch (err) {
      console.error(err);
    }
  };

  const handleAcknowledgeAlert = async (alertId: number) => {
    try {
      await fetch("/api/alerts", {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          alertId,
          action: "ACKNOWLEDGE",
          acknowledgedBy: "Növbətçi Triaq Həkimi",
        }),
      });
      await fetchAlerts();
    } catch (err) {
      console.error(err);
    }
  };

  const handlePatientCreated = (ticketData: PrintableTicketData) => {
    setPrintableData(ticketData);
    fetchAlerts();
  };

  return (
    <div className="min-h-screen bg-[#06090E] text-slate-100 flex flex-col font-sans">
      {/* Perimeter emergency warning overlay and audio synthesizer */}
      <EmergencyOverlay
        alerts={alerts}
        onAcknowledge={handleAcknowledgeAlert}
        onRefreshNeeded={fetchAlerts}
      />

      {/* Main command bar */}
      <Header
        activeTab={activeTab}
        setActiveTab={setActiveTab}
        activeAlertCount={alerts.length}
        onTriggerPanic={handleTriggerPanic}
        onOpenQueueMonitor={() => setIsQueueMonitorOpen(true)}
      />

      {/* Main Container */}
      <main className="flex-1 max-w-[1720px] w-full mx-auto p-4 sm:p-6 space-y-6">
        {activeTab === "reception" && (
          <ReceptionTriage
            onPatientCreated={handlePatientCreated}
            onEmergencyTriggered={fetchAlerts}
          />
        )}

        {activeTab === "emergency" && (
          <EmergencyTerminal onRefreshNeeded={fetchAlerts} />
        )}

        {activeTab === "cabinets" && <CabinetDispatch />}

        {activeTab === "audit" && (
          <AuditStatistics onPrintPatient={(data) => setPrintableData(data)} />
        )}
      </main>

      {/* Public Hospital Queue Monitor Modal */}
      <QueueMonitorModal
        isOpen={isQueueMonitorOpen}
        onClose={() => setIsQueueMonitorOpen(false)}
      />

      {/* Printable Referral Ticket Modal */}
      <PrintReferralTicket
        data={printableData}
        onClose={() => setPrintableData(null)}
      />

      {/* Footer Meta */}
      <footer className="border-t border-slate-900 bg-[#070B12] px-6 py-3 text-xs text-slate-500 flex flex-wrap items-center justify-between gap-2">
        <div className="flex items-center gap-2">
          <span className="font-semibold text-slate-400">MedTriage Pro Enterprise</span>
          <span>•</span>
          <span>Klinik Qəbul və Manchester Triage Sistemi Terminalı</span>
          <span>•</span>
          <span className="text-emerald-400 font-mono">SQLite Portable Edition (clinic.db)</span>
        </div>
        <div className="flex items-center gap-4 text-[11px]">
          <span>Mərkəzi Server: Localhost:3000</span>
          <span>Standart: MTS Clinical Rule Engine</span>
        </div>
      </footer>
    </div>
  );
}
