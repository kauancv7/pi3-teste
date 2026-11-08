import Link from "next/link";

import { Container } from "@/components/layout/Container";
import { LoginForm } from "@/features/auth/components/login-form";

export default function LoginPage() {
  return (
    <Container className="py-14 sm:py-20">
      <section className="mx-auto max-w-md border border-[#e2ddd5] bg-white p-6 shadow-soft sm:p-8">
        <p className="text-[11px] font-black uppercase tracking-[0.2em] text-[#8b5e3c]">
          Lojas Stuart
        </p>
        <h1 className="mt-2 text-3xl font-black text-[#231c19]">Entrar na conta</h1>
        <p className="mt-3 text-sm leading-6 text-[#7c726b]">
          Acesse sua conta para continuar suas compras.
        </p>

        <div className="mt-7">
          <LoginForm />
        </div>

        <p className="mt-6 text-center text-sm text-[#7c726b]">
          Ainda não possui conta?{" "}
          <Link href="/cadastro" className="font-bold text-[#8b4513] underline">
            Cadastre-se
          </Link>
        </p>
      </section>
    </Container>
  );
}
