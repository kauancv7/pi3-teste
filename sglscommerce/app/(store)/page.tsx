import {
  CategorySection,
  FeaturedProductsSection,
  HomeCta,
  HomeHero,
  NewsletterSection,
  PromoBannersSection,
} from "@/features/home/components/home-sections";

export default function HomePage() {
  return (
    <>
      <HomeHero />
      <CategorySection />
      <FeaturedProductsSection />
      <PromoBannersSection />
      <NewsletterSection />
      <HomeCta />
    </>
  );
}