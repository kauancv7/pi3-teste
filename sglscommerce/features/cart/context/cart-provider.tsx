"use client";

import {
  createContext,
  type Dispatch,
  type ReactNode,
  useMemo,
  useReducer,
} from "react";

import type { Cart } from "@/features/cart/types/cart";
import {
  cartReducer,
  createEmptyCart,
  type CartAction,
} from "@/features/cart/state/cart-reducer";

type CartContextValue = {
  cart: Cart;
  dispatch: Dispatch<CartAction>;
};

export const CartContext = createContext<CartContextValue | null>(null);

type CartProviderProps = {
  children: ReactNode;
};

export function CartProvider({ children }: CartProviderProps) {
  const [cart, dispatch] = useReducer(cartReducer, undefined, createEmptyCart);
  const value = useMemo(() => ({ cart, dispatch }), [cart]);

  return <CartContext value={value}>{children}</CartContext>;
}
