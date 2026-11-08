"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

import { ProductDetail } from "@/features/products/components/product-detail";
import { getPublicProducts } from "@/features/products/services/product-service";
import type { Product } from "@/features/products/types/product";

type ProductDetailRouteProps = {
  slug: string;
};

export function ProductDetailRoute({ slug }: ProductDetailRouteProps) {
  const [product, setProduct] = useState<Product | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(false);
  const [reloadKey, setReloadKey] = useState(0);

  useEffect(() => {
    let isCurrentRequest = true;

    async function loadProduct() {
      setIsLoading(true);
      setError(false);

      try {
        const products = await getPublicProducts();
        const foundProduct = products.find((item) => item.slug === slug);

        if (isCurrentRequest) {
          setProduct(foundProduct ?? null);
        }
      } catch (loadError) {
        console.error("Erro ao carregar os detalhes do produto:", loadError);
        if (isCurrentRequest) {
          setError(true);
        }
      } finally {
        if (isCurrentRequest) {
          setIsLoading(false);
        }
      }
    }

    void loadProduct();

    return () => {
      isCurrentRequest = false;
    };
  }, [reloadKey, slug]);

  if (isLoading) {
    return (
      <main className="min-h-[50vh] px-4 py-16 text-center text-[#7c726b]">
        <p role="status">Carregando produto...</p>
      </main>
    );
  }

  if (error) {
    return (
      <main className="min-h-[50vh] px-4 py-16 text-center text-[#7c726b]">
        <p role="alert">Não foi possível carregar este produto.</p>
        <button
          type="button"
          onClick={() => setReloadKey((key) => key + 1)}
          className="mt-4 border border-[#2d221e] px-4 py-2 font-bold text-[#2d221e] transition-colors hover:bg-[#2d221e] hover:text-white"
        >
          Tentar novamente
        </button>
      </main>
    );
  }

  if (!product) {
    return (
      <main className="min-h-[50vh] px-4 py-16 text-center text-[#7c726b]">
        <h1 className="text-xl font-bold text-[#231c19]">
          Produto não encontrado
        </h1>
        <p className="mt-2">Este produto pode ter sido removido ou estar indisponível.</p>
        <Link
          href="/products"
          className="mt-4 inline-block font-bold text-[#8b4513] underline"
        >
          Voltar para produtos
        </Link>
      </main>
    );
  }

  return <ProductDetail product={product} />;
}
