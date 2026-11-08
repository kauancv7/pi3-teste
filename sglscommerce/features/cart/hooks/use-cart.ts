"use client";

import { useCallback, useContext } from "react";

import { CartContext } from "@/features/cart/context/cart-provider";
import {
  getSubtotalCents,
  getTotalCents,
  getTotalItems,
} from "@/features/cart/state/cart-reducer";
import type { CartProductInput } from "@/features/cart/types/cart";

export function useCart() {
  const context = useContext(CartContext);

  if (!context) {
    throw new Error("useCart deve ser usado dentro de CartProvider");
  }

  const { cart, dispatch } = context;

  const addItem = useCallback(
    (input: CartProductInput) => dispatch({ type: "addItem", product: input }),
    [dispatch],
  );

  const removeItem = useCallback(
    (key: string) => dispatch({ type: "removeItem", key }),
    [dispatch],
  );

  const setQuantity = useCallback(
    (key: string, quantity: number) =>
      dispatch({ type: "setQuantity", key, quantity }),
    [dispatch],
  );

  const clearCart = useCallback(
    () => dispatch({ type: "clearCart" }),
    [dispatch],
  );

  return {
    cart,
    addItem,
    removeItem,
    setQuantity,
    clearCart,
    totalItems: getTotalItems(cart.items),
    subtotalCents: getSubtotalCents(cart.items),
    totalCents: getTotalCents(cart.items),
  };
}
