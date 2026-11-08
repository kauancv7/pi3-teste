"use client";

import { useEffect, useState } from "react";
import { api } from "@/services/api";
import { ProductsPage } from "@/features/products/components/products-page";

import type { Product } from "@/features/products/types/product";

interface Produto {
  id: string;
  nome: string;
  descricao: string;
  preco: number;
  estoque: number;
  ativo: boolean;
  categoriaId: string;
  categoriaNome: string;
  imagemId: string;
}

export default function ProductsRoute() {
  const [produtos, setProdutos] = useState<Product[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState("");

  useEffect(() => {
    async function carregarProdutos() {
      try {
        const resposta = await api.get<Produto[]>("/produtos/publicos");

        // Define a URL base da API
        const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5150/api";

        const produtosConvertidos: Product[] = resposta.data
          .filter((produto) => produto.ativo)
          .map((produto) => ({
            id: produto.id,
            name: produto.nome,
            slug: produto.id,
            price: produto.preco,
            category: produto.categoriaNome,
            image: {
              src: produto.imagemId ? `${baseUrl}/images/${produto.imagemId}/download` : undefined,
              alt: produto.nome,
              label: "Lojas Stuart",
            },
          }));

        setProdutos(produtosConvertidos);
      } catch (error) {
        console.error("Erro ao buscar produtos:", error);
        setErro("Não foi possível carregar os produtos. Tente novamente.");
      } finally {
        setCarregando(false);
      }
    }

    carregarProdutos();
  }, []);

  if (carregando) {
    return (
      <main className="min-h-screen bg-[#f8f9fa] p-8 text-[#231c19]">
        Carregando produtos...
      </main>
    );
  }

  if (erro) {
    return (
      <main className="min-h-screen bg-[#f8f9fa] p-8 text-[#231c19]">
        <p>{erro}</p>
        <button
          onClick={() => window.location.reload()}
          className="mt-4 bg-[#2d221e] px-6 py-3 text-white"
        >
          Tentar novamente
        </button>
      </main>
    );
  }

  return <ProductsPage products={produtos} />;
}