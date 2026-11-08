import { api } from "@/services/api";
import type { Product } from "@/features/products/types/product";

type ApiProduct = {
  id: string;
  nome: string;
  descricao: string;
  preco: number;
  estoque: number;
  ativo: boolean;
  categoriaNome: string;
  imagemId: string | null;
};

export async function getPublicProducts(): Promise<Product[]> {
  const { data } = await api.get<ApiProduct[]>("/produtos/publicos");
  const apiBaseUrl = api.defaults.baseURL?.replace(/\/$/, "");

  return data
    .filter((product) => product.ativo)
    .map((product) => ({
      id: product.id,
      name: product.nome,
      slug: product.id,
      price: product.preco,
      description: product.descricao,
      stock: product.estoque,
      category: product.categoriaNome,
      image: {
        src:
          product.imagemId && apiBaseUrl
            ? `${apiBaseUrl}/images/${product.imagemId}/download`
            : undefined,
        alt: product.nome,
        label: product.nome.slice(0, 2).toUpperCase() || "PR",
      },
    }));
}
