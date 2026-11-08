import Link from "next/link";

import { Container } from "@/components/layout/Container";
import { Badge } from "@/components/ui/badge";
import { AddToCartButton } from "@/features/cart/components/add-to-cart-button";
import { Card, CardContent } from "@/components/ui/card";
import { Typography } from "@/components/ui/typography";
import { type Product } from "@/features/products/types/product";
import { toCartProductInput } from "@/features/cart/utils/to-cart-product-input";

const priceFormatter = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

type ProductDetailProps = {
  product: Product;
};

export function ProductDetail({ product }: ProductDetailProps) {
  const installmentValue = priceFormatter.format(product.price / 6);
  const pixPrice = priceFormatter.format(product.price * 0.93);
  const cartProduct = toCartProductInput(product);

  return (
    <section className="py-8 sm:py-12 bg-[#f8f9fa]">
      <Container>
        {/* Breadcrumbs */}
        <nav aria-label="Navegação estrutural">
          <ol className="flex flex-wrap items-center gap-x-2 gap-y-1 text-xs text-[#9e9389] font-medium uppercase tracking-wider">
            <li>
              <Link
                href="/"
                className="transition-colors hover:text-[#2d221e] focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-[#2d221e]"
              >
                Início
              </Link>
            </li>
            <li aria-hidden="true">/</li>
            <li>
              <Link
                href="/products"
                className="transition-colors hover:text-[#2d221e] focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-[#2d221e]"
              >
                Produtos
              </Link>
            </li>
            <li aria-hidden="true">/</li>
            <li aria-current="page" className="min-w-0 truncate font-bold text-[#231c19]">
              {product.name}
            </li>
          </ol>
        </nav>

        <div className="mt-8 grid gap-8 lg:grid-cols-2 lg:items-start lg:gap-12">
          {/* Image Container */}
          <div className="relative overflow-hidden border border-[#e2ddd5] bg-[#f3f1ed] flex aspect-[4/5] items-center justify-center">
            <div className="absolute top-3 left-3 flex flex-col gap-1.5 z-10">
              {product.isNew && (
                <span className="bg-[#2d221e] px-2.5 py-1 text-[11px] font-black text-[#f8f9fa] uppercase tracking-wider">
                  NOVO
                </span>
              )}
              {product.discountBadge && (
                <span className="bg-[#8b4513] px-2.5 py-1 text-[11px] font-black text-white uppercase tracking-wider">
                  {product.discountBadge}
                </span>
              )}
            </div>

            {product.image.src ? (
              <img
                src={product.image.src}
                alt={product.image.alt}
                className="h-full w-full object-cover object-center"
              />
            ) : (
              <span className="flex size-28 items-center justify-center bg-white text-3xl font-black tracking-widest text-[#2d221e] shadow-soft">
                {product.image.label}
              </span>
            )}
          </div>

          {/* Product Info */}
          <div className="min-w-0">
            <Badge variant="secondary" className="font-bold uppercase tracking-wider text-[10px] bg-[#eeeae4] text-[#2d221e] border border-[#e2ddd5]">
              {product.category}
            </Badge>
            <Typography variant="h1" className="mt-3 text-2xl sm:text-3xl font-black text-[#231c19] uppercase tracking-tight leading-tight">
              {product.name}
            </Typography>

            {/* Pricing Section - Warm Earth Standard */}
            <div className="mt-6 rounded-none bg-white p-6 border border-[#e2ddd5] shadow-soft">
              <div className="flex items-baseline gap-3">
                {product.originalPrice && (
                  <span className="text-sm text-[#9e9389] line-through font-normal">
                    {priceFormatter.format(product.originalPrice)}
                  </span>
                )}
                <span className="text-3xl sm:text-4xl font-black text-[#231c19]">
                  {priceFormatter.format(product.price)}
                </span>
              </div>

              <div className="mt-3 space-y-1.5 text-xs text-[#7c726b]">
                <p>
                  💳 ou até <strong>6x de {installmentValue}</strong> sem juros nos cartões
                </p>
                <p className="text-[#8b4513] font-bold">
                  ⚡ <strong>{pixPrice}</strong> no PIX (7% de desconto exclusivo)
                </p>
              </div>
            </div>

            {/* Product Summary */}
            <div className="mt-6 border-t border-[#e2ddd5] pt-6">
              <Typography variant="h3" className="text-xs font-black text-[#2d221e] uppercase tracking-widest">
                Descrição do Produto
              </Typography>
              <Typography className="mt-2 text-xs sm:text-sm text-[#7c726b] leading-relaxed">
                {product.description || "Descrição não disponível."}
              </Typography>
              {product.stock !== undefined && (
                <p className="mt-3 text-xs font-bold text-[#7c726b]">
                  {product.stock > 0
                    ? `${product.stock} unidade${product.stock === 1 ? "" : "s"} em estoque`
                    : "Produto indisponível no momento"}
                </p>
              )}
            </div>

            {/* Quick Benefits */}
            <div className="mt-6 grid grid-cols-2 gap-3 text-xs">
              <div className="border border-[#e2ddd5] bg-white p-3 flex items-center gap-2.5">
                <TruckIcon className="size-4 shrink-0 text-[#8b4513]" />
                <div>
                  <p className="font-bold uppercase text-[11px] text-[#231c19]">Frete Grátis</p>
                  <p className="text-[10px] text-[#9e9389]">Acima de R$ 199</p>
                </div>
              </div>

              <div className="border border-[#e2ddd5] bg-white p-3 flex items-center gap-2.5">
                <RefreshIcon className="size-4 shrink-0 text-[#8b4513]" />
                <div>
                  <p className="font-bold uppercase text-[11px] text-[#231c19]">Troca Fácil</p>
                  <p className="text-[10px] text-[#9e9389]">30 dias para troca</p>
                </div>
              </div>
            </div>

            {/* Add to Cart Section */}
            <Card className="mt-6 rounded-none border border-[#e2ddd5] bg-white">
              <CardContent className="p-5">
                <Typography as="h2" variant="h3" className="text-xs font-black uppercase tracking-widest text-[#231c19]">
                  Finalizar Compra
                </Typography>
                <AddToCartButton
                  product={cartProduct}
                  disabled={product.stock === 0}
                />
              </CardContent>
            </Card>
          </div>
        </div>
      </Container>
    </section>
  );
}

function TruckIcon({ className }: { className?: string }) {
  return (
    <svg aria-hidden="true" className={className} fill="none" stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.75" viewBox="0 0 24 24">
      <path d="M10 17h4V5H2v12h3" />
      <path d="M20 17h2v-3.34a4 4 0 0 0-1.17-2.83L19 9h-5v8h1" />
      <circle cx="7.5" cy="17.5" r="2.5" />
      <circle cx="17.5" cy="17.5" r="2.5" />
    </svg>
  );
}

function RefreshIcon({ className }: { className?: string }) {
  return (
    <svg aria-hidden="true" className={className} fill="none" stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.75" viewBox="0 0 24 24">
      <path d="M21 12a9 9 0 0 0-9-9 9.75 9.75 0 0 0-6.74 2.74L3 8" />
      <path d="M3 3v5h5" />
      <path d="M3 12a9 9 0 0 0 9 9 9.75 9.75 0 0 0 6.74-2.74L21 16" />
      <path d="M16 21h5v-5" />
    </svg>
  );
}
