"use client";

import { useEffect, useRef, useState } from "react";

import { Button } from "@/components/ui/button";
import { useCart } from "@/features/cart/hooks/use-cart";
import type { CartProductInput } from "@/features/cart/types/cart";

type AddToCartButtonProps = {
  product: CartProductInput;
  disabled?: boolean;
};

const FEEDBACK_TIMEOUT_MS = 1500;

export function AddToCartButton({ product, disabled = false }: AddToCartButtonProps) {
  const { addItem } = useCart();
  const [wasAdded, setWasAdded] = useState(false);
  const feedbackTimeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  useEffect(() => {
    return () => {
      if (feedbackTimeoutRef.current) {
        clearTimeout(feedbackTimeoutRef.current);
      }
    };
  }, []);

  function handleClick() {
    addItem(product);
    setWasAdded(true);

    if (feedbackTimeoutRef.current) {
      clearTimeout(feedbackTimeoutRef.current);
    }

    feedbackTimeoutRef.current = setTimeout(() => {
      setWasAdded(false);
      feedbackTimeoutRef.current = null;
    }, FEEDBACK_TIMEOUT_MS);
  }

  return (
    <Button
      type="button"
      size="lg"
      variant="action"
      className="mt-5 w-full rounded-none font-bold uppercase tracking-wider"
      onClick={handleClick}
      disabled={disabled}
      aria-live="polite"
    >
      {disabled
        ? "Produto indisponível"
        : wasAdded
          ? "Adicionado ao Carrinho ✓"
          : "Adicionar ao Carrinho"}
    </Button>
  );
}
