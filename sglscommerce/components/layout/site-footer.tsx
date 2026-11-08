import Link from "next/link";
import { Container } from "@/components/layout/Container";

export function SiteFooter() {
  const currentYear = new Date().getFullYear();

  return (
    <footer className="border-t border-[#2e2420] bg-[#1c1513] text-[#b3a89f]">
      {/* Main Footer Links */}
      <div className="py-12 sm:py-16">
        <Container className="grid grid-cols-1 gap-10 sm:grid-cols-2 lg:grid-cols-4">
          {/* Brand Info */}
          <div>
            <div className="flex items-center gap-3">
              <span className="flex size-10 items-center justify-center bg-[#f8f9fa] font-black text-base text-[#2d221e] tracking-widest">
                LS
              </span>
              <span className="text-lg font-black tracking-wider text-[#f8f9fa] uppercase font-sans">
                LOJAS STUART
              </span>
            </div>
            <p className="mt-4 text-xs leading-relaxed text-[#9e9389]">
              Tradição e excelência em alfaiataria, peças essenciais e moda masculina & feminina premium. Atendimento diferenciado e entregas para todo o Brasil.
            </p>

            <div className="mt-6 flex items-center gap-2.5 text-xs">
              <a href="#" className="flex size-8 items-center justify-center border border-[#3e2f28] bg-[#251d1a] text-[#e8e2d8] transition-colors hover:border-[#f8f9fa] hover:text-[#f8f9fa]" aria-label="Instagram">
                <InstagramOutlineIcon className="size-4" />
              </a>
              <a href="#" className="flex size-8 items-center justify-center border border-[#3e2f28] bg-[#251d1a] text-[#e8e2d8] transition-colors hover:border-[#f8f9fa] hover:text-[#f8f9fa]" aria-label="Facebook">
                <FacebookOutlineIcon className="size-4" />
              </a>
              <a href="#" className="flex size-8 items-center justify-center border border-[#3e2f28] bg-[#251d1a] text-[#e8e2d8] transition-colors hover:border-[#f8f9fa] hover:text-[#f8f9fa]" aria-label="WhatsApp">
                <WhatsAppOutlineIcon className="size-4" />
              </a>
            </div>
          </div>

          {/* Institutional Links */}
          <div>
            <h4 className="text-xs font-black uppercase tracking-[0.15em] text-[#f8f9fa]">Institucional</h4>
            <ul className="mt-4 space-y-2.5 text-xs text-[#9e9389]">
              <li>
                <Link href="/" className="hover:text-[#f8f9fa] transition-colors">
                  Sobre as Lojas Stuart
                </Link>
              </li>
              <li>
                <Link href="/" className="hover:text-[#f8f9fa] transition-colors">
                  Nossas Lojas Físicas
                </Link>
              </li>
              <li>
                <Link href="/" className="hover:text-[#f8f9fa] transition-colors">
                  Trabalhe Conosco
                </Link>
              </li>
              <li>
                <Link href="/" className="hover:text-[#f8f9fa] transition-colors">
                  Política de Privacidade
                </Link>
              </li>
              <li>
                <Link href="/" className="hover:text-[#f8f9fa] transition-colors">
                  Termos e Condições
                </Link>
              </li>
            </ul>
          </div>

          {/* Help & Support */}
          <div>
            <h4 className="text-xs font-black uppercase tracking-[0.15em] text-[#f8f9fa]">Ajuda & Suporte</h4>
            <ul className="mt-4 space-y-2.5 text-xs text-[#9e9389]">
              <li>
                <Link href="/" className="hover:text-[#f8f9fa] transition-colors">
                  Central de Atendimento
                </Link>
              </li>
              <li>
                <Link href="/" className="hover:text-[#f8f9fa] transition-colors">
                  Trocas e Devoluções
                </Link>
              </li>
              <li>
                <Link href="/" className="hover:text-[#f8f9fa] transition-colors">
                  Entregas e Frete
                </Link>
              </li>
              <li>
                <Link href="/" className="hover:text-[#f8f9fa] transition-colors">
                  Perguntas Frequentes (FAQ)
                </Link>
              </li>
              <li>
                <Link href="/" className="hover:text-[#f8f9fa] transition-colors">
                  Meus Pedidos
                </Link>
              </li>
            </ul>
          </div>

          {/* Payment & Security */}
          <div>
            <h4 className="text-xs font-black uppercase tracking-[0.15em] text-[#f8f9fa]">Formas de Pagamento</h4>
            <div className="mt-4 flex flex-wrap gap-2 text-[11px]">
              <span className="bg-[#2a201c] border border-[#3e2f28] px-2.5 py-1 font-bold text-[#c68b59]">PIX (7% OFF)</span>
              <span className="bg-[#2a201c] border border-[#3e2f28] px-2.5 py-1 font-medium text-[#d6cec5]">Cartão de Crédito</span>
              <span className="bg-[#2a201c] border border-[#3e2f28] px-2.5 py-1 font-medium text-[#d6cec5]">Boleto</span>
              <span className="bg-[#2a201c] border border-[#3e2f28] px-2.5 py-1 font-medium text-[#d6cec5]">Até 6x Sem Juros</span>
            </div>

            <h4 className="mt-6 text-xs font-black uppercase tracking-[0.15em] text-[#f8f9fa]">Segurança</h4>
            <div className="mt-3 flex items-center gap-2 text-xs text-[#d6cec5]">
              <ShieldCheckOutlineIcon className="size-4 text-[#c68b59]" />
              <span>SSL 256-bits | Compra 100% Protegida</span>
            </div>
          </div>
        </Container>
      </div>

      {/* Bottom Copyright */}
      <div className="border-t border-[#2e2420] py-6 text-xs text-[#7c726b]">
        <Container className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
          <p>© {currentYear} Lojas Stuart. Todos os direitos reservados.</p>
          <p className="text-[#7c726b]">SGLS Commerce • Base Headless integrada ao PostgreSQL</p>
        </Container>
      </div>
    </footer>
  );
}

function InstagramOutlineIcon({ className }: { className?: string }) {
  return (
    <svg aria-hidden="true" className={className} fill="none" stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.75" viewBox="0 0 24 24">
      <rect width="20" height="20" x="2" y="2" rx="5" ry="5" />
      <path d="M16 11.37A4 4 0 1 1 12.63 8 4 4 0 0 1 16 11.37z" />
      <line x1="17.5" x2="17.51" y1="6.5" y2="6.5" />
    </svg>
  );
}

function FacebookOutlineIcon({ className }: { className?: string }) {
  return (
    <svg aria-hidden="true" className={className} fill="none" stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.75" viewBox="0 0 24 24">
      <path d="M18 2h-3a5 5 0 0 0-5 5v3H7v4h3v8h4v-8h3l1-4h-4V7a1 1 0 0 1 1-1h3z" />
    </svg>
  );
}

function WhatsAppOutlineIcon({ className }: { className?: string }) {
  return (
    <svg aria-hidden="true" className={className} fill="none" stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.75" viewBox="0 0 24 24">
      <path d="M21 11.5a8.38 8.38 0 0 1-.9 3.8 8.5 8.5 0 0 1-7.6 4.7 8.38 8.38 0 0 1-3.8-.9L3 21l1.9-5.7a8.38 8.38 0 0 1-.9-3.8 8.5 8.5 0 0 1 4.7-7.6 8.38 8.38 0 0 1 3.8-.9h.5a8.48 8.48 0 0 1 8 8v.5z" />
    </svg>
  );
}

function ShieldCheckOutlineIcon({ className }: { className?: string }) {
  return (
    <svg aria-hidden="true" className={className} fill="none" stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.75" viewBox="0 0 24 24">
      <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
      <path d="m9 12 2 2 4-4" />
    </svg>
  );
}
