import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "MedTriage Pro — Klinik Qəbul, Ekspert Diaqnostika və Təcili Həyəcan Terminalı",
  description: "Müasir tibb müəssisələri üçün Manchester Triaq Sistemi (MTS) və təcili dispetçer idarəetmə platforması",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="az" className="dark h-full">
      <body className="min-h-full bg-[#06090E] text-slate-100 antialiased selection:bg-rose-500 selection:text-white">
        {children}
      </body>
    </html>
  );
}
