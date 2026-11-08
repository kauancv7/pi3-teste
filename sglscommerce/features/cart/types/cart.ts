/** Serializable cart state persisted independently from catalog presentation models. */
export type Cart = {
  schemaVersion: 1;
  currency: "BRL";
  items: CartItem[];
};

export type CartItem = {
  key: string;
  productId: string;
  variantId?: string;
  sku?: string;
  quantity: number;
  unitPriceCents: number;
  productSnapshot: {
    name: string;
    slug: string;
    image: {
      alt: string;
      label: string;
    };
  };
};

/** Minimum catalog information required to create a cart item. */
export type CartProductInput = {
  productId: string;
  variantId?: string;
  sku?: string;
  name: string;
  slug: string;
  image: {
    alt: string;
    label: string;
  };
  price: number;
};
