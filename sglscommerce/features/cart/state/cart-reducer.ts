import type { Cart, CartItem, CartProductInput } from "@/features/cart/types/cart";

export const CART_SCHEMA_VERSION = 1 as const;
export const CART_CURRENCY = "BRL" as const;

export type CartAction =
  | { type: "addItem"; product: CartProductInput }
  | { type: "removeItem"; key: string }
  | { type: "setQuantity"; key: string; quantity: number }
  | { type: "clearCart" };

export function createEmptyCart(): Cart {
  return {
    schemaVersion: CART_SCHEMA_VERSION,
    currency: CART_CURRENCY,
    items: [],
  };
}

/**
 * Returns the sellable-unit identity for the current catalog model.
 * Variant and SKU fields are retained in the model for a future identity evolution.
 */
export function getCartItemKey(productId: string): string {
  assertNonEmptyString(productId, "productId");

  return `${productId}:default`;
}

/** Converts a catalog price expressed in reais to integer centavos. */
export function toPriceCents(price: number): number {
  if (!Number.isFinite(price) || price < 0) {
    throw new Error("price must be a non-negative finite number");
  }

  const priceCents = Math.round(price * 100);

  if (!Number.isSafeInteger(priceCents)) {
    throw new Error("price is outside the supported range");
  }

  return priceCents;
}

export function createCartItem(product: CartProductInput): CartItem {
  assertNonEmptyString(product.productId, "productId");
  assertNonEmptyString(product.name, "name");
  assertNonEmptyString(product.slug, "slug");
  assertNonEmptyString(product.image.alt, "image.alt");
  assertNonEmptyString(product.image.label, "image.label");

  if (product.variantId !== undefined) {
    assertNonEmptyString(product.variantId, "variantId");
  }

  if (product.sku !== undefined) {
    assertNonEmptyString(product.sku, "sku");
  }

  return {
    key: getCartItemKey(product.productId),
    productId: product.productId,
    ...(product.variantId === undefined ? {} : { variantId: product.variantId }),
    ...(product.sku === undefined ? {} : { sku: product.sku }),
    quantity: 1,
    unitPriceCents: toPriceCents(product.price),
    productSnapshot: {
      name: product.name,
      slug: product.slug,
      image: {
        alt: product.image.alt,
        label: product.image.label,
      },
    },
  };
}

export function cartReducer(state: Cart, action: CartAction): Cart {
  switch (action.type) {
    case "addItem": {
      const item = createCartItem(action.product);
      const existingItem = state.items.find((cartItem) => cartItem.key === item.key);

      if (existingItem) {
        return {
          ...state,
          items: state.items.map((cartItem) =>
            cartItem.key === item.key
              ? { ...cartItem, quantity: cartItem.quantity + 1 }
              : cartItem,
          ),
        };
      }

      return { ...state, items: [...state.items, item] };
    }

    case "removeItem":
      if (!state.items.some((item) => item.key === action.key)) {
        return state;
      }

      return {
        ...state,
        items: state.items.filter((item) => item.key !== action.key),
      };

    case "setQuantity":
      if (!Number.isInteger(action.quantity)) {
        return state;
      }

      if (action.quantity <= 0) {
        return cartReducer(state, { type: "removeItem", key: action.key });
      }

      if (!state.items.some((item) => item.key === action.key)) {
        return state;
      }

      return {
        ...state,
        items: state.items.map((item) =>
          item.key === action.key ? { ...item, quantity: action.quantity } : item,
        ),
      };

    case "clearCart":
      return { ...state, items: [] };
  }
}

export function getTotalItems(items: CartItem[]): number {
  return items.reduce((totalItems, item) => totalItems + item.quantity, 0);
}

export function getSubtotalCents(items: CartItem[]): number {
  return items.reduce(
    (subtotalCents, item) => subtotalCents + item.quantity * item.unitPriceCents,
    0,
  );
}

/** The initial cart has no additional charges, so total equals subtotal. */
export function getTotalCents(items: CartItem[]): number {
  return getSubtotalCents(items);
}

function assertNonEmptyString(value: string, fieldName: string): void {
  if (value.trim().length === 0) {
    throw new Error(`${fieldName} must not be empty`);
  }
}
