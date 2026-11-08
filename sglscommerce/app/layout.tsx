
import type { Metadata } from "next";
import { type ReactNode } from "react";

import { AppProviders } from "@/providers/app-providers";

import "./globals.css";

export const metadata: Metadata = {
  title: "SGLS Commerce",
  description: "E-commerce headless SGLS Commerce",
};

type RootLayoutProps = {
  children: ReactNode;
};

export default function RootLayout({ children }: RootLayoutProps) {
  return (
    <html lang="pt-BR" className="h-full antialiased">
      <body className="flex min-h-full flex-col">
        <AppProviders>
          <main className="flex flex-1 flex-col bg-background">
            {children}
          </main>
        </AppProviders>
      </body>
    </html>
  );
}
