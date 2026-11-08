"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

import { LoginForm } from "@/features/auth/components/login-form";

export function LoginDrawer() {
  const [open, setOpen] = useState(false);

  useEffect(() => {
    document.body.style.overflow = open ? "hidden" : "";

    return () => {
      document.body.style.overflow = "";
    };
  }, [open]);

  return (
    <>
      {/* BOTÃO MINHA CONTA */}
      <button
        type="button"
        onClick={() => setOpen(true)}
        className="inline-flex h-10 items-center justify-center gap-2 px-2 text-xs font-semibold text-[#2d221e] transition-colors hover:text-[#8b4513]"
        aria-label="Minha conta"
      >
        <UserIcon className="size-5" />

        <div className="flex flex-col text-left leading-tight">
          <span className="text-[10px] font-medium uppercase text-[#9e9389]">
            Entrar
          </span>

          <span className="text-xs font-bold uppercase">
            Minha Conta
          </span>
        </div>
      </button>

      {/* FUNDO ESCURECIDO */}
      <div
        onClick={() => setOpen(false)}
        className={`fixed inset-0 z-[80] bg-black/60 transition-opacity duration-300 ${
          open
            ? "pointer-events-auto opacity-100"
            : "pointer-events-none opacity-0"
        }`}
      />

      {/* PAINEL LATERAL */}
      <aside
        className={`fixed right-0 top-0 z-[90] h-screen w-full max-w-[460px] bg-white shadow-2xl transition-transform duration-300 ease-in-out ${
          open ? "translate-x-0" : "translate-x-full"
        }`}
        aria-hidden={!open}
      >
        {/* CABEÇALHO */}
        <div className="flex h-16 items-center justify-between border-b border-[#e2ddd5] px-6">
          <h2 className="text-lg font-black uppercase text-[#231c19]">
            Entrar
          </h2>

          <button
            type="button"
            onClick={() => setOpen(false)}
            className="flex items-center gap-2 text-xs font-bold uppercase text-[#231c19] transition-colors hover:text-[#8b4513]"
          >
            <CloseIcon className="size-4" />
            Fechar
          </button>
        </div>

        {/* FORMULÁRIO */}
        <div className="px-6 py-7">
          <LoginForm onSuccess={() => setOpen(false)} />
        </div>

        {/* CRIAR CONTA */}
        <div className="border-t border-[#e2ddd5] px-6 py-10 text-center">
          <UserLargeIcon className="mx-auto size-14 text-[#dedbd7]" />

          <p className="mt-4 text-sm font-bold text-[#231c19]">
            Não tem conta?
          </p>

         <Link
          href="/cadastro"
          className="mt-4 inline-block border-b border-[#231c19] pb-1 text-xs font-black uppercase tracking-wider text-[#231c19] transition-colors hover:border-[#8b4513] hover:text-[#8b4513]"
         >
         Criar conta
         </Link>
        </div>
      </aside>
    </>
  );
}

type IconProps = {
  className?: string;
};

function UserIcon({ className }: IconProps) {
  return (
    <svg
      className={className}
      fill="none"
      stroke="currentColor"
      strokeWidth="1.75"
      viewBox="0 0 24 24"
    >
      <path
        strokeLinecap="round"
        strokeLinejoin="round"
        d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2"
      />
      <circle cx="12" cy="7" r="4" />
    </svg>
  );
}

function UserLargeIcon({ className }: IconProps) {
  return (
    <svg
      className={className}
      fill="none"
      stroke="currentColor"
      strokeWidth="1.4"
      viewBox="0 0 24 24"
    >
      <circle cx="12" cy="8" r="4" />
      <path
        strokeLinecap="round"
        strokeLinejoin="round"
        d="M4.5 21a7.5 7.5 0 0 1 15 0"
      />
    </svg>
  );
}

function CloseIcon({ className }: IconProps) {
  return (
    <svg
      className={className}
      fill="none"
      stroke="currentColor"
      strokeWidth="1.75"
      viewBox="0 0 24 24"
    >
      <path
        strokeLinecap="round"
        d="M6 6l12 12M18 6 6 18"
      />
    </svg>
  );
}
