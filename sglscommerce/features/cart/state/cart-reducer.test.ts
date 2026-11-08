import assert from "node:assert/strict";
import test from "node:test";

import type { CartProductInput } from "../types/cart.ts";
import {
  cartReducer,
  createEmptyCart,
  getSubtotalCents,
  getTotalCents,
  getTotalItems,
  toPriceCents,
} from "./cart-reducer.ts";

const product: CartProductInput = {
  productId: "demo-headphone",
  name: "Fone Aurora",
  slug: "fone-aurora",
  image: { alt: "Placeholder do Fone Aurora", label: "FA" },
  price: 249.9,
};

test("adds a new product as a cart item", () => {
  const cart = cartReducer(createEmptyCart(), { type: "addItem", product });

  assert.deepEqual(cart.items, [
    {
      key: "demo-headphone:default",
      productId: "demo-headphone",
      quantity: 1,
      unitPriceCents: 24990,
      productSnapshot: {
        name: "Fone Aurora",
        slug: "fone-aurora",
        image: { alt: "Placeholder do Fone Aurora", label: "FA" },
      },
    },
  ]);
});

test("increments quantity when the same product is added twice", () => {
  const once = cartReducer(createEmptyCart(), { type: "addItem", product });
  const twice = cartReducer(once, { type: "addItem", product });

  assert.equal(twice.items.length, 1);
  assert.equal(twice.items[0].quantity, 2);
});

test("removes a product by key", () => {
  const cart = cartReducer(createEmptyCart(), { type: "addItem", product });
  const nextCart = cartReducer(cart, {
    type: "removeItem",
    key: "demo-headphone:default",
  });

  assert.deepEqual(nextCart.items, []);
});

test("sets an item quantity", () => {
  const cart = cartReducer(createEmptyCart(), { type: "addItem", product });
  const nextCart = cartReducer(cart, {
    type: "setQuantity",
    key: "demo-headphone:default",
    quantity: 3,
  });

  assert.equal(nextCart.items[0].quantity, 3);
});

test("removes an item when its quantity is zero or negative", () => {
  const cart = cartReducer(createEmptyCart(), { type: "addItem", product });
  const emptyFromZero = cartReducer(cart, {
    type: "setQuantity",
    key: "demo-headphone:default",
    quantity: 0,
  });
  const emptyFromNegative = cartReducer(cart, {
    type: "setQuantity",
    key: "demo-headphone:default",
    quantity: -1,
  });

  assert.deepEqual(emptyFromZero.items, []);
  assert.deepEqual(emptyFromNegative.items, []);
});

test("clears items while preserving cart metadata", () => {
  const cart = cartReducer(createEmptyCart(), { type: "addItem", product });
  const nextCart = cartReducer(cart, { type: "clearCart" });

  assert.deepEqual(nextCart, createEmptyCart());
});

test("derives total item quantity and subtotal in centavos", () => {
  const cart = cartReducer(
    cartReducer(createEmptyCart(), { type: "addItem", product }),
    { type: "setQuantity", key: "demo-headphone:default", quantity: 2 },
  );

  assert.equal(getTotalItems(cart.items), 2);
  assert.equal(getSubtotalCents(cart.items), 49980);
  assert.equal(getTotalCents(cart.items), 49980);
  assert.equal(toPriceCents(249.9), 24990);
});

test("creates new state and item references without mutating previous cart data", () => {
  const originalCart = cartReducer(createEmptyCart(), { type: "addItem", product });
  const nextCart = cartReducer(originalCart, {
    type: "setQuantity",
    key: "demo-headphone:default",
    quantity: 2,
  });

  assert.notStrictEqual(nextCart, originalCart);
  assert.notStrictEqual(nextCart.items, originalCart.items);
  assert.notStrictEqual(nextCart.items[0], originalCart.items[0]);
  assert.equal(originalCart.items[0].quantity, 1);
  assert.equal(nextCart.items[0].quantity, 2);
});
