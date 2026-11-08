import { Suspense } from "react";
import Link from "next/link";

import { Container } from "@/components/layout/Container";
import { ProductGrid } from "@/features/products/components/product-grid";
import { getPublicProducts } from "@/features/products/services/product-service";
import type { Product } from "@/features/products/types/product";

type SearchPageProps = {
  searchParams: Promise<{ q?: string | string[] }>;
};

function normalize(value: string) {
  return value.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase();
}

function SearchLoading() {
  return (
    <main className="bg-[#f8f9fa] py-12 sm:py-16">
      <Container>
        <p role="status" className="text-sm text-[#7c726b]">
          Buscando produtos...
        </p>
      </Container>
    </main>
  );
}

async function SearchResults({ searchParams }: SearchPageProps) {
  const { q } = await searchParams;
  const query = (Array.isArray(q) ? q[0] : q)?.trim() ?? "";
  let products: Product[] = [];
  let error = false;

  try {
    const allProducts = await getPublicProducts();
    const normalizedQuery = normalize(query);
    products = normalizedQuery
      ? allProducts.filter((product) =>
          normalize(`${product.name} ${product.category}`).includes(normalizedQuery),
        )
      : allProducts;
  } catch (loadError) {
    console.error("Erro ao buscar produtos:", loadError);
    error = true;
  }

  return (
    <main className="bg-[#f8f9fa] py-12 sm:py-16">
      <Container>
        <p className="text-[11px] font-black uppercase tracking-[0.2em] text-[#8b5e3c]">
          Busca
        </p>
        <h1 className="mt-2 text-3xl font-black text-[#231c19] sm:text-4xl">
          {query ? `Resultados para “${query}”` : "Todos os produtos"}
        </h1>

        <div className="mt-8">
          {error && (
            <p role="alert" className="text-sm text-[#b93826]">
              Não foi possível realizar a busca.
            </p>
          )}
          {!error && products.length === 0 && (
            <div className="border border-[#e2ddd5] bg-white p-8 text-center">
              <p className="font-bold text-[#231c19]">Nenhum produto encontrado.</p>
              <Link
                href="/products"
                className="mt-4 inline-block font-bold text-[#8b4513] underline"
              >
                Ver catálogo completo
              </Link>
            </div>
          )}
          {!error && products.length > 0 && <ProductGrid products={products} />}
        </div>
      </Container>
    </main>
  );
}

export default function SearchPage(props: SearchPageProps) {
  return (
    <Suspense fallback={<SearchLoading />}>
      <SearchResults {...props} />
    </Suspense>
  );
}
