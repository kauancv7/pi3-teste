import Link from "next/link";

import { Container } from "@/components/layout/Container";

export default function CadastroPage() {
  return (
    <main className="bg-white">
      <Container className="py-14 sm:py-16 lg:py-20">
        <div className="mx-auto grid max-w-5xl gap-12 lg:grid-cols-2 lg:gap-0">

          {/* LADO ESQUERDO - CADASTRO */}
          <section className="lg:pr-14">
            <h1 className="text-2xl font-black text-[#231c19]">
              Cadastre-se
            </h1>

            <form className="mt-8">
              <div>
                <label
                  htmlFor="email"
                  className="mb-2 block text-sm font-semibold text-[#231c19]"
                >
                  Endereço de e-mail{" "}
                  <span className="text-[#8b4513]">*</span>
                </label>

                <input
                  id="email"
                  name="email"
                  type="email"
                  required
                  placeholder="Digite seu e-mail"
                  className="h-12 w-full border border-[#ddd7d0] bg-white px-4 text-sm text-[#231c19] outline-none transition-colors placeholder:text-[#a39a92] focus:border-[#2d221e]"
                />
              </div>

              <p className="mt-5 text-sm leading-relaxed text-[#5f5752]">
                Um link para definir uma nova senha será enviado para o seu
                endereço de e-mail.
              </p>

              <p className="mt-6 text-sm leading-relaxed text-[#5f5752]">
                Seus dados pessoais serão utilizados para melhorar sua
                experiência em nosso site, gerenciar o acesso à sua conta e
                para outros propósitos descritos em nossa política de
                privacidade.
              </p>

              <button
                type="button"
                className="mt-7 h-12 w-full bg-[#2d221e] text-xs font-black uppercase tracking-wider !text-white transition-colors hover:bg-[#42322c]"
              >
                Cadastre-se
              </button>
            </form>
          </section>

          {/* LADO DIREITO - JÁ TEM CONTA */}
          <section className="border-t border-[#e2ddd5] pt-10 text-center lg:border-l lg:border-t-0 lg:pl-14 lg:pt-0">
            <h2 className="text-2xl font-black text-[#231c19]">
              Entrar
            </h2>

            <p className="mx-auto mt-8 max-w-md text-sm leading-relaxed text-[#5f5752]">
              Se você já possui uma conta, entre para acompanhar seus pedidos,
              acessar seus dados e tornar o processo de compra mais rápido e
              fácil.
            </p>

            <Link
              href="/"
              className="mt-7 inline-flex h-11 items-center justify-center border border-[#2d221e] bg-[#2d221e] px-8 text-xs font-black uppercase tracking-wider !text-white transition-colors hover:bg-[#42322c]"
            >
              Entrar
            </Link>
          </section>

        </div>
      </Container>
    </main>
  );
}