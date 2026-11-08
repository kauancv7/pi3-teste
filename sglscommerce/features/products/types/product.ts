/**
 * Modelo de apresentação temporário para o catálogo do storefront.
 * Não representa o contrato definitivo da SGLS API.
 */
export type Product = {
  id: string;
  name: string;
  slug: string;
  price: number;
  description?: string;
  stock?: number;
  originalPrice?: number;
  category: string;
  discountBadge?: string;
  isNew?: boolean;
  image: {
    src?: string;
    alt: string;
    label: string;
  };
};
