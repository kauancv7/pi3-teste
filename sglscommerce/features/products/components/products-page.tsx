import { Container } from "@/components/layout/Container";
import { Badge } from "@/components/ui/badge";
import { Typography } from "@/components/ui/typography";
import { ProductGrid } from "@/features/products/components/product-grid";
import { type Product } from "@/features/products/types/product";

type ProductsPageProps = {
  products: Product[];
};

export function ProductsPage({ products }: ProductsPageProps) {
  return (
    <main>
      <section className="border-b border-border bg-accent py-16 sm:py-20" aria-labelledby="products-title">
        <Container>
          <Badge variant="secondary">Catálogo demonstrativo</Badge>
          <Typography id="products-title" variant="h1" className="mt-5">
            Produtos
          </Typography>
          <Typography className="mt-4 max-w-2xl text-lg">
            Explore nossa seleção temporária enquanto o catálogo definitivo é preparado para integração.
          </Typography>
        </Container>
      </section>

      <section className="py-12 sm:py-16" aria-labelledby="products-grid-title">
        <Container>
          <div className="flex flex-col gap-3 border-b border-border pb-6 sm:flex-row sm:items-end sm:justify-between">
            <div>
              <Typography id="products-grid-title" variant="h2">
                Todos os produtos
              </Typography>
              <Typography variant="small" className="mt-2">
                {products.length} {products.length === 1 ? "produto disponível" : "produtos disponíveis"}
              </Typography>
            </div>
            <Typography variant="small">Filtros e ordenação serão disponibilizados em breve.</Typography>
          </div>
          <div className="mt-8">
            <ProductGrid products={products} />
          </div>
        </Container>
      </section>
    </main>
  );
}
