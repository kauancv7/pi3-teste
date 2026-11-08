import { type ReactNode } from "react";

import { Header } from "@/components/layout/Header";
import { SiteFooter } from "@/components/layout/site-footer";

const initialCategories = [
  { name: "Kits", href: "/products?category=kits" },
  { name: "Camisas", href: "/products?category=camisas" },
  { name: "Calças", href: "/products?category=calcas" },
  { name: "Sapatos", href: "/products?category=sapatos" },
  { name: "Vestidos", href: "/products?category=vestidos" },
  { name: "Acessórios", href: "/products?category=acessorios" },
  { name: "Sandálias / saltos", href: "/products?category=sandalias-saltos" },
];

type StoreLayoutProps = {
  children: ReactNode;
};

export default function StoreLayout({ children }: StoreLayoutProps) {
  return (
    <>
      <Header categories={initialCategories} />
      {children}
      <SiteFooter />
    </>
  );
}