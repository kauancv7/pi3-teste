import Link from "next/link";

import { Card, CardContent } from "@/components/ui/card";
import { Typography } from "@/components/ui/typography";
import { type Product } from "@/features/products/types/product";

const priceFormatter = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

type ProductCardProps = {
  product: Product;
};

export function ProductCard({ product }: ProductCardProps) {
  const installmentValue = priceFormatter.format(product.price / 3);

  return (
    <article className="group h-full">
      <Link
        href={`/products/${product.slug}`}
        className="block h-full focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-[#2d221e] focus-visible:ring-offset-2"
      >
        <Card className="relative flex h-full flex-col overflow-hidden rounded-none border border-[#e2ddd5] bg-white transition-all duration-200 hover:border-[#2d221e] hover:shadow-card">
          {/* Badges Container */}
          <div className="absolute left-2.5 top-2.5 z-10 flex flex-col gap-1">
            {product.isNew && (
              <span className="bg-[#2d221e] px-2 py-0.5 text-[10px] font-black uppercase tracking-wider text-[#f8f9fa]">
                NOVO
              </span>
            )}

            {product.discountBadge && (
              <span className="bg-[#8b4513] px-2 py-0.5 text-[10px] font-black uppercase tracking-wider text-white">
                {product.discountBadge}
              </span>
            )}
          </div>

          {/* Image Container */}
          <div
            className="relative flex aspect-[4/5] w-full items-center justify-center overflow-hidden bg-[#f3f1ed]"
            role="img"
            aria-label={product.image.alt}
          >
            {product.image.src ? (
              <img
                src={product.image.src}
                alt={product.image.alt}
                loading="lazy"
                className="h-full w-full object-cover object-center transition-transform duration-500 group-hover:scale-105"
              />
            ) : (
              <span className="flex size-16 items-center justify-center bg-white text-xl font-black tracking-widest text-[#2d221e] shadow-soft transition-transform duration-300 group-hover:scale-105">
                {product.image.label}
              </span>
            )}
          </div>

          <CardContent className="flex flex-1 flex-col p-4">
            <Typography
              as="p"
              variant="small"
              className="text-[10px] font-bold uppercase tracking-widest text-[#9e9389]"
            >
              {product.category}
            </Typography>

            <Typography
              as="h3"
              variant="h3"
              className="mt-1 line-clamp-2 text-sm font-bold uppercase tracking-tight text-[#231c19] transition-colors group-hover:text-[#8b4513]"
            >
              {product.name}
            </Typography>

            <div className="mt-auto pt-3">
              <div className="flex items-baseline gap-2">
                {product.originalPrice && (
                  <span className="text-xs text-[#9e9389] line-through">
                    {priceFormatter.format(product.originalPrice)}
                  </span>
                )}

                <span className="text-base font-black text-[#231c19]">
                  {priceFormatter.format(product.price)}
                </span>
              </div>

              <p className="mt-1 text-[11px] font-medium text-[#7c726b]">
                até <strong>3x</strong> de{" "}
                <strong className="text-[#2d221e]">
                  {installmentValue}
                </strong>{" "}
                sem juros
              </p>
            </div>

            {/* Quick Action Button */}
            <div className="mt-3 border-t border-[#eeeae4] pt-2">
              <span className="inline-flex w-full items-center justify-center border border-[#e2ddd5] py-1.5 text-[10px] font-black uppercase tracking-wider text-[#2d221e] transition-colors group-hover:border-[#2d221e] group-hover:bg-[#2d221e] group-hover:text-[#f8f9fa]">
                Ver opções
              </span>
            </div>
          </CardContent>
        </Card>
      </Link>
    </article>
  );
}