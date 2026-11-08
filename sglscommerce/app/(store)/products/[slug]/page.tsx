import type { Metadata } from "next";

import { ProductDetailRoute } from "@/features/products/components/product-detail-route";

type ProductDetailPageProps = {
  params: Promise<{ slug: string }>;
};

export const metadata: Metadata = {
  title: "Detalhes do produto | Lojas Stuart",
};

export default async function ProductDetailPage({
  params,
}: ProductDetailPageProps) {
  const { slug } = await params;

  return <ProductDetailRoute slug={slug} />;
}
