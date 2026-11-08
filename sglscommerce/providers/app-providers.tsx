"use client";

import { type ReactNode } from "react";
import { CartProvider } from "@/features/cart/context/cart-provider";
import { QueryProvider } from "@/providers/query-provider";

type AppProvidersProps = {
  children: ReactNode;
};

export function AppProviders({ children }: AppProvidersProps) {
  return (
    <QueryProvider>
      <CartProvider>{children}</CartProvider>
    </QueryProvider>
  );
}
