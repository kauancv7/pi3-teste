"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

const navItems = [
  { name: "Início", href: "/", icon: HomeIcon },
  { name: "Kits", href: "/products?category=kits", icon: PackageIcon },
  { name: "Camisas", href: "/products?category=camisas", icon: ShirtIcon },
  { name: "Calças", href: "/products?category=calcas", icon: PantsIcon },
  { name: "Sapatos", href: "/products?category=sapatos", icon: ShoeIcon },
  { name: "Vestidos", href: "/products?category=vestidos", icon: DressIcon },
  { name: "Acessórios", href: "/products?category=acessorios", icon: GlassesIcon },
  { name: "Sandálias / Saltos", href: "/products?category=sandalias-saltos", icon: HeelIcon },
];

export function Navbar() {
  const pathname = usePathname();

  return (
    <nav className="border-b border-[#e2ddd5] bg-white sticky top-0 z-30 shadow-xs">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-start space-x-1 overflow-x-auto py-3 scrollbar-none">
          {navItems.map((item) => {
            const Icon = item.icon;
            // Removemos a verificação de ativo para o Início ou para todos se preferir que nenhum escureça,
            // mas aqui mantemos o comportamento apenas desativando o fundo escuro para o "Início" (href === '/')
            const isActive = item.href !== "/" && pathname === item.href;

            return (
              <Link
                key={item.name}
                href={item.href}
                className={`flex items-center gap-2 px-4 py-2 text-xs font-black uppercase tracking-wider transition-colors whitespace-nowrap ${
                  isActive
                    ? "bg-[#231c19] text-white"
                    : "text-[#231c19] hover:bg-[#f8f9fa] hover:text-[#8b4513]"
                }`}
              >
                <Icon className="size-4 shrink-0" />
                <span>{item.name}</span>
              </Link>
            );
          })}
        </div>
      </div>
    </nav>
  );
}

// Ícones SVG inline
function HomeIcon({ className }: { className?: string }) {
  return (
    <svg className={className} fill="none" stroke="currentColor" strokeWidth="1.75" viewBox="0 0 24 24">
      <path strokeLinecap="round" strokeLinejoin="round" d="m3 9 9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z" />
      <polyline points="9 22 9 12 15 12 15 22" />
    </svg>
  );
}

function PackageIcon({ className }: { className?: string }) {
  return (
    <svg className={className} fill="none" stroke="currentColor" strokeWidth="1.75" viewBox="0 0 24 24">
      <path strokeLinecap="round" strokeLinejoin="round" d="m16.5 9.4-9-5.19M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z" />
      <polyline points="3.27 6.96 12 12.01 20.73 6.96" />
      <line x1="12" y1="22.08" x2="12" y2="12" />
    </svg>
  );
}

function ShirtIcon({ className }: { className?: string }) {
  return (
    <svg className={className} fill="none" stroke="currentColor" strokeWidth="1.75" viewBox="0 0 24 24">
      <path strokeLinecap="round" strokeLinejoin="round" d="M20.38 3.46 16 2a4 4 0 0 1-8 0L3.62 3.46a2 2 0 0 0-1.34 2.23l.58 3.47a1 1 0 0 0 .99.84H5v10a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V10h1.15a1 1 0 0 0 .99-.84l.58-3.47a2 2 0 0 0-1.34-2.23z" />
    </svg>
  );
}

function PantsIcon({ className }: { className?: string }) {
  return (
    <svg className={className} fill="none" stroke="currentColor" strokeWidth="1.75" viewBox="0 0 24 24">
      <path strokeLinecap="round" strokeLinejoin="round" d="M9 3h6l2 18H14l-2-7-2 7H7L9 3z" />
    </svg>
  );
}

function ShoeIcon({ className }: { className?: string }) {
  return (
    <svg className={className} fill="none" stroke="currentColor" strokeWidth="1.75" viewBox="0 0 24 24">
      <path strokeLinecap="round" strokeLinejoin="round" d="M4 16v1a3 3 0 0 0 3 3h10a3 3 0 0 0 3-3v-1m-16-4h16l-1.5-6h-13z" />
    </svg>
  );
}

function DressIcon({ className }: { className?: string }) {
  return (
    <svg className={className} fill="none" stroke="currentColor" strokeWidth="1.75" viewBox="0 0 24 24">
      <path strokeLinecap="round" strokeLinejoin="round" d="m15 2-3 2-3-2-4 7 3 13h10l3-13z" />
    </svg>
  );
}

function GlassesIcon({ className }: { className?: string }) {
  return (
    <svg className={className} fill="none" stroke="currentColor" strokeWidth="1.75" viewBox="0 0 24 24">
      <circle cx="6" cy="15" r="4" />
      <circle cx="18" cy="15" r="4" />
      <path strokeLinecap="round" strokeLinejoin="round" d="M10 15h4m-8-2-2-7h3l2 7m10-2 2-7h-3l-2 7" />
    </svg>
  );
}

function HeelIcon({ className }: { className?: string }) {
  return (
    <svg className={className} fill="none" stroke="currentColor" strokeWidth="1.75" viewBox="0 0 24 24">
      <path strokeLinecap="round" strokeLinejoin="round" d="M3 16h11l4-4V8h-2l-4 4H5v3a1 1 0 0 1-1 1H3zM14 16l4 5h3" />
    </svg>
  );
}