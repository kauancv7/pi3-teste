import assert from "node:assert/strict";
import test from "node:test";

import { toCartProductInput } from "./to-cart-product-input.ts";
import type { Product } from "../../products/types/product.ts";

test("maps a product to cart product input", () => {
  const product: Product = {
    id: "camiseta-linho",
    name: "Camiseta Linho",
    slug: "camiseta-linho",
    price: 129.9,
    originalPrice: 159.9,
    category: "Camisetas",
    discountBadge: "-19%",
    isNew: true,
    image: {
      alt: "Camiseta Linho em fundo neutro",
      label: "CL",
    },
  };

  assert.deepEqual(toCartProductInput(product), {
    productId: "camiseta-linho",
    name: "Camiseta Linho",
    slug: "camiseta-linho",
    image: {
      alt: "Camiseta Linho em fundo neutro",
      label: "CL",
    },
    price: 129.9,
  });
});
