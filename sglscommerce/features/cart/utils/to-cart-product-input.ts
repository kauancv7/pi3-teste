import type { CartProductInput } from "@/features/cart/types/cart";
import type { Product } from "@/features/products/types/product";

export function toCartProductInput(product: Product): CartProductInput {
  return {
    productId: product.id,
    name: product.name,
    slug: product.slug,
    image: {
      alt: product.image.alt,
      label: product.image.label,
    },
    price: product.price,
  };
}
