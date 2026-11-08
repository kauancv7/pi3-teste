import { type Product } from "@/features/products/types/product";

/** Dados locais temporários para a apresentação do catálogo com imagens reais do MVP. */
export const mockProducts: Product[] = [
  {
    id: "stuart-blazer-slim",
    name: "Blazer Alfaiataria Slim Fit",
    slug: "blazer-alfaiataria-slim-fit",
    price: 289.9,
    originalPrice: 349.9,
    discountBadge: "17% OFF",
    isNew: true,
    category: "Masculino",
    image: {
      src: "/images/temp/prod-1.jpeg",
      alt: "Blazer Alfaiataria Slim Fit Lojas Stuart",
      label: "BL",
    },
  },
  {
    id: "stuart-vestido-linho",
    name: "Vestido Midi Linho Premium",
    slug: "vestido-midi-linho-premium",
    price: 219.9,
    originalPrice: 269.9,
    discountBadge: "18% OFF",
    isNew: true,
    category: "Feminino",
    image: {
      src: "/images/temp/prod-2.jpeg",
      alt: "Vestido Midi Linho Premium Lojas Stuart",
      label: "VD",
    },
  },
  {
    id: "stuart-camisa-oxford",
    name: "Camisa Social Oxford Algodão",
    slug: "camisa-social-oxford-algodao",
    price: 149.9,
    originalPrice: 179.9,
    discountBadge: "16% OFF",
    category: "Masculino",
    image: {
      src: "/images/temp/prod-3.jpeg",
      alt: "Camisa Social Oxford Algodão Lojas Stuart",
      label: "CS",
    },
  },
  {
    id: "stuart-jaqueta-couro",
    name: "Jaqueta Puffer Outono Stuart",
    slug: "jaqueta-puffer-outono-stuart",
    price: 329.9,
    originalPrice: 399.9,
    discountBadge: "17% OFF",
    isNew: true,
    category: "Casacos & Jaquetas",
    image: {
      src: "/images/temp/prod-4.jpeg",
      alt: "Jaqueta Puffer Outono Lojas Stuart",
      label: "JQ",
    },
  },
  {
    id: "stuart-calca-pantacourt",
    name: "Calça Pantalona Alfaiataria",
    slug: "calca-pantalona-alfaiataria",
    price: 179.9,
    category: "Feminino",
    image: {
      src: "/images/temp/prod-5.jpeg",
      alt: "Calça Pantalona Alfaiataria Lojas Stuart",
      label: "CP",
    },
  },
  {
    id: "stuart-tricot-gola-alta",
    name: "Suéter Tricot Gola Alta",
    slug: "sueter-tricot-gola-alta",
    price: 199.9,
    originalPrice: 239.9,
    discountBadge: "16% OFF",
    category: "Inverno",
    image: {
      src: "/images/temp/prod-6.jpeg",
      alt: "Suéter Tricot Gola Alta Lojas Stuart",
      label: "ST",
    },
  },
  {
    id: "stuart-polo-tech",
    name: "Camisa Polo Tech Piquet",
    slug: "camisa-polo-tech-piquet",
    price: 159.9,
    originalPrice: 189.9,
    discountBadge: "15% OFF",
    category: "Masculino",
    image: {
      src: "/images/temp/prod-7.jpeg",
      alt: "Camisa Polo Tech Piquet Lojas Stuart",
      label: "PL",
    },
  },
];

export function getMockProductBySlug(slug: string) {
  return mockProducts.find((product) => product.slug === slug);
}
