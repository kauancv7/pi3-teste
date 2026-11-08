"use client";

import { type FormEvent, useEffect, useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";

import { Container } from "@/components/layout/Container";
import { Input } from "@/components/ui/input";
import { useCart } from "@/features/cart/hooks/use-cart";

// 1. Tipagem para a categoria que virá do servidor
export type Category = {
  name: string;
  href: string;
};

// 2. O Header agora recebe as categorias via props
type HeaderProps = {
  categories: Category[];
};

export function Header({ categories }: HeaderProps) {
  const { totalItems } = useCart();
  const router = useRouter();

  const [searchTerm, setSearchTerm] = useState("");
  const [menuOpen, setMenuOpen] = useState(false);
  const [searchOpen, setSearchOpen] = useState(false);

  // Fecha o menu com ESC e impede a rolagem da página enquanto estiver aberto.
  useEffect(() => {
    if (!menuOpen) return;

    document.body.style.overflow = "hidden";

    function handleKeyDown(event: KeyboardEvent) {
      if (event.key === "Escape") {
        setMenuOpen(false);
      }
    }

    window.addEventListener("keydown", handleKeyDown);

    return () => {
      document.body.style.overflow = "";
      window.removeEventListener("keydown", handleKeyDown);
    };
  }, [menuOpen]);

  function handleSearch(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const query = searchTerm.trim();

    router.push(
      query ? `/busca?q=${encodeURIComponent(query)}` : "/busca"
    );

    setSearchOpen(false);
  }

  function closeMenu() {
    setMenuOpen(false);
  }

  return (
    <>
      {/* Cabeçalho principal */}
      <header className="sticky top-0 z-40 w-full">
        <div className="relative bg-[#EAD9A6] text-[#573817]">
<div className="relative mx-auto flex h-[72px] w-full items-center justify-between px-5 sm:px-8 lg:px-8">
           {/* Botão MENU */}
            <button
              type="button"
              onClick={() => {
                setMenuOpen(true);
                setSearchOpen(false);
              }}
              aria-label="Abrir menu"
              aria-expanded={menuOpen}
              className="flex items-center gap-3 transition-opacity hover:opacity-70"
            >
              <MenuIcon className="size-6" />

              <span className="mt-1 text-sm font-medium uppercase tracking-wide">
                Menu
              </span>
            </button>

            {/* Logo centralizada */}
            <Link
              href="/"
              aria-label="Lojas Stuart - página inicial"
              className="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2"
            >
              <img
                src="/logo-stuart.png"
                alt="Lojas Stuart"
                className="h-11 w-auto object-contain"
              />
            </Link>

            {/* Ações à direita */}
            <div className="flex items-center gap-5">

              {/* Carrinho */}
              <Link
                href="/carrinho"
                aria-label={`Carrinho com ${totalItems} produtos`}
                className="relative transition-opacity hover:opacity-70"
              >
                <ShoppingCartIcon className="size-6" />

                {totalItems > 0 && (
                  <span className="absolute -right-2 -top-2 flex size-4 items-center justify-center rounded-full bg-[#573817] text-[10px] font-bold text-white">
                    {totalItems}
                  </span>
                )}
              </Link>

              {/* Busca */}
              <button
                type="button"
                onClick={() => setSearchOpen(!searchOpen)}
                aria-label="Abrir busca"
                aria-expanded={searchOpen}
                className="transition-opacity hover:opacity-70"
              >
                {searchOpen ? (
                  <CloseIcon className="size-6" />
                ) : (
                  <SearchOutlineIcon className="size-6" />
                )}
              </button>
            </div>
          </div>
        </div>

        {/* Busca expansível */}
        {searchOpen && (
          <div className="border-t border-[#d5c38e] bg-[#EAD9A6] py-4">
            <Container>
              <form
                onSubmit={handleSearch}
                className="flex w-full gap-2"
                role="search"
              >
                <Input
                  type="search"
                  name="search"
                  value={searchTerm}
                  onChange={(event) => setSearchTerm(event.target.value)}
                  placeholder="O que você procura?"
                  aria-label="Buscar produtos"
                  autoFocus
                  className="h-11 flex-1 rounded-none border-[#c8b783] bg-white text-[#573817] placeholder:text-[#927d56] focus:border-[#573817] focus:ring-0"
                />

                <button
                  type="submit"
                  className="bg-[#573817] px-6 text-xs font-bold uppercase tracking-wider text-white transition-colors hover:bg-[#76502b]"
                >
                  Buscar
                </button>
              </form>
            </Container>
          </div>
        )}
      </header>

      {/* Fundo escuro atrás do menu */}
      {menuOpen && (
        <button
          type="button"
          aria-label="Fechar menu"
          onClick={closeMenu}
          className="fixed inset-0 z-[60] cursor-default bg-black/20"
        />
      )}

      {/* Menu lateral */}
      <aside
        id="main-menu"
        role="dialog"
        aria-modal="true"
        aria-label="Menu de navegação"
        aria-hidden={!menuOpen}
        className={`
          fixed inset-y-0 left-0 z-[70]
          flex h-[100dvh] flex-col
          w-[52vw] min-w-[250px] max-w-[360px]
          bg-[#E5E2E2] text-black
          shadow-xl
          transition-transform duration-300 ease-in-out
          ${menuOpen ? "translate-x-0" : "-translate-x-full"}
        `}
      >
        {/* Cabeçalho do menu */}
        <div className="flex h-[130px] shrink-0 items-center justify-between border-b border-[#777] px-6">
          <h2 className="text-[23px] font-bold uppercase">
            Menu
          </h2>

          <button
            type="button"
            onClick={closeMenu}
            aria-label="Fechar menu"
            className="flex size-8 items-center justify-center rounded-full border-2 border-black transition-colors hover:bg-black hover:text-white"
          >
            <CloseIcon className="size-5" />
          </button>
        </div>

        {/* Categorias */}
        <nav
          aria-label="Categorias"
          className="flex-1 overflow-y-auto px-2"
        >
          {/* 3. Mapeando as categorias que vieram via props */}
          {categories.map((item) => (
            <Link
              key={item.name}
              href={item.href}
              onClick={closeMenu}
              className="flex min-h-[56px] items-center border-b border-[#777] px-5 text-[22px] font-normal capitalize transition-colors hover:bg-[#d7d3d3]"
            >
              {item.name}
            </Link>
          ))}

          {/* Login */}
          <Link
            href="/login"
            onClick={closeMenu}
            className="flex min-h-[56px] items-center px-5 text-[22px] font-bold transition-colors hover:bg-[#d7d3d3]"
          >
            Login
          </Link>
        </nav>
      </aside>
    </>
  );
}

/* Ícones SVG */

type IconProps = {
  className?: string;
};

function MenuIcon({ className }: IconProps) {
  return (
    <svg
      aria-hidden="true"
      className={className}
      fill="none"
      stroke="currentColor"
      strokeLinecap="round"
      strokeWidth="1.7"
      viewBox="0 0 24 24"
    >
      <path d="M4 6h16M4 12h16M4 18h16" />
    </svg>
  );
}

function CloseIcon({ className }: IconProps) {
  return (
    <svg
      aria-hidden="true"
      className={className}
      fill="none"
      stroke="currentColor"
      strokeLinecap="round"
      strokeLinejoin="round"
      strokeWidth="1.8"
      viewBox="0 0 24 24"
    >
      <path d="m18 6-12 12M6 6l12 12" />
    </svg>
  );
}

function SearchOutlineIcon({ className }: IconProps) {
  return (
    <svg
      aria-hidden="true"
      className={className}
      fill="none"
      stroke="currentColor"
      strokeLinecap="round"
      strokeLinejoin="round"
      strokeWidth="1.75"
      viewBox="0 0 24 24"
    >
      <circle cx="11" cy="11" r="8" />
      <path d="m21 21-4.35-4.35" />
    </svg>
  );
}

function ShoppingCartIcon({ className }: IconProps) {
  return (
    <svg
      aria-hidden="true"
      className={className}
      fill="none"
      stroke="currentColor"
      strokeLinecap="round"
      strokeLinejoin="round"
      strokeWidth="1.75"
      viewBox="0 0 24 24"
    >
      <circle cx="9" cy="21" r="1" />
      <circle cx="20" cy="21" r="1" />
      <path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6" />
    </svg>
  );
}