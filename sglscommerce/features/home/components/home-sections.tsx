"use client";

import { useEffect, useState } from "react";

import Link from "next/link";

import Image from "next/image";

import { Container } from "@/components/layout/Container";

import { ProductGrid } from "@/features/products/components/product-grid";

import { getPublicProducts } from "@/features/products/services/product-service";

import type { Product } from "@/features/products/types/product";

const banners = [

  {

    image: "/images/temp/prod-1.jpeg",

    title: "Banner 1",

  },

  {

    image: "/images/temp/prod-2.jpeg",

    title: "Banner 2",

  },

  {

    image: "/images/temp/prod-3.jpeg",

    title: "Banner 3",

  },

];

export function HomeHero() {

const [currentSlide, setCurrentSlide] = useState(0);

  useEffect(() => {

const timer = setInterval(() => {

      setCurrentSlide((prev) => (prev + 1) % banners.length);

    }, 5000);

    return () => clearInterval(timer);

  }, []);

  return (

    <section

      className="relative h-[400px] w-full overflow-hidden border-b border-[#e2ddd5] bg-[#231c19] sm:h-[500px] lg:h-[550px]"

      aria-label="Carrossel de Destaques"

    >

      <div className="absolute inset-0 z-0">

        {banners.map((banner, index) => (

          <div

            key={banner.image}

            className={`absolute inset-0 transition-opacity duration-1000 ease-in-out ${

              index === currentSlide

                ? "z-10 opacity-100"

                : "z-0 opacity-0"

            }`}

          >

            <Image

              src={banner.image}

              alt={banner.title}

              fill

              className="object-cover object-center"

              priority={index === 0}

            />

          </div>

        ))}

      </div>

      <div className="absolute bottom-6 left-1/2 z-20 flex -translate-x-1/2 gap-2">

        {banners.map((banner, index) => (

          <button

            key={banner.image}

            onClick={() => setCurrentSlide(index)}

            className={`h-2.5 rounded-full shadow-md transition-all ${

              index === currentSlide

                ? "w-8 bg-[#8b4513]"

                : "w-2.5 bg-white/60 hover:bg-white"

            }`}

            aria-label={`Ir para slide ${index + 1}`}

          />

        ))}

      </div>

    </section>

  );

}

type StoreCategory = {

  name: string;

  image?: Product["image"];

  productCount: number;

};

function obterRotaCategoria(nome: string): string {

const nomeNormalizado = nome

    .normalize("NFD")

    .replace(/[\u0300-\u036f]/g, "")

    .trim()

    .toLowerCase();

  // Reconhece "Casacos", "CASACOS", "Casacos e Jaquetas" etc.

  if (nomeNormalizado.includes("casac")) {

    return "/categorias/casacos";

  }

  if (nomeNormalizado.includes("mascul")) {

    return "/categorias/masculino";

  }

  if (nomeNormalizado.includes("feminin")) {

    return "/categorias/feminino";

  }

  if (nomeNormalizado.includes("acessor")) {

    return "/categorias/acessorios";

  }

  return "/products";

}

export function CategorySection() {
  const [categories, setCategories] = useState<StoreCategory[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(false);

  useEffect(() => {
    let isCurrentRequest = true;

    getPublicProducts()
      .then((products) => {
        if (!isCurrentRequest) return;

        const groupedCategories = new Map<string, StoreCategory>();

        products.forEach((product) => {
          const name = product.category.trim();
          if (!name) return;

          const key = name
            .normalize("NFD")
            .replace(/[\u0300-\u036f]/g, "")
            .toLowerCase();

          const current = groupedCategories.get(key);

          groupedCategories.set(key, {
            name,
            image: current?.image ?? product.image,
            productCount: (current?.productCount ?? 0) + 1,
          });
        });

        setCategories(Array.from(groupedCategories.values()).slice(0, 6));
      })
      .catch((loadError: unknown) => {
        console.error("Erro ao carregar categorias:", loadError);
        if (isCurrentRequest) setError(true);
      })
      .finally(() => {
        if (isCurrentRequest) setIsLoading(false);
      });

    return () => {
      isCurrentRequest = false;
    };
  }, []);

  return (
    <section
      id="categories"
      className="bg-[#e7e4e4] py-14 sm:py-16 lg:py-20"
      aria-labelledby="categories-title"
    >
      <Container>
        <div className="relative mx-auto max-w-5xl border-4 border-[#593006] px-4 pb-7 pt-12 sm:px-8 sm:pb-9 sm:pt-14">
          <h2
            id="categories-title"
            className="absolute left-1/2 top-0 -translate-x-1/2 -translate-y-1/2 whitespace-nowrap bg-[#593006] px-8 py-2 text-center text-lg font-semibold text-white sm:px-12 sm:text-xl"
          >
            Categorias
          </h2>

          <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-3 lg:gap-8">
            {isLoading && (
              <p role="status" className="col-span-full text-center text-sm text-[#7c726b]">
                Carregando categorias...
              </p>
            )}

            {!isLoading && error && (
              <p role="alert" className="col-span-full text-center text-sm text-[#b93826]">
                Não foi possível carregar as categorias.
              </p>
            )}

            {!isLoading &&
              !error &&
              categories.map((category) => {
                const rota = obterRotaCategoria(category.name);

                return (
                  <Link
                    key={category.name}
                    href={rota}
                    className="group relative block overflow-hidden rounded-2xl bg-[#d4d0cc] shadow-sm transition-transform duration-300 hover:-translate-y-1 hover:shadow-lg"
                    aria-label={`${category.name}: ${category.productCount} produtos`}
                  >
                    <div className="relative aspect-[4/5] w-full overflow-hidden">
                      {category.image?.src ? (
                        <img
                          src={category.image.src}
                          alt={category.image.alt || category.name}
                          className="h-full w-full object-cover object-center transition-transform duration-500 group-hover:scale-105"
                        />
                      ) : (
                        <div className="flex h-full w-full items-center justify-center bg-[#d8d3ce]">
                          <span className="text-2xl font-bold uppercase text-[#593006]">
                            {category.name.slice(0, 2)}
                          </span>
                        </div>
                      )}

                      <div className="absolute inset-0 bg-gradient-to-t from-black/65 via-transparent to-transparent" />

                      <span className="absolute bottom-4 left-4 text-lg font-extrabold uppercase text-white [text-shadow:1px_2px_3px_rgba(0,0,0,0.8)] sm:text-xl">
                        {category.name}
                      </span>
                    </div>
                  </Link>
                );
              })}
          </div>
        </div>
      </Container>
    </section>
  );
}

export function FeaturedProductsSection() {
  const [products, setProducts] = useState<Product[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(false);
  const [reloadKey, setReloadKey] = useState(0);

  useEffect(() => {
    let isCurrentRequest = true;

    getPublicProducts()
      .then((publicProducts) => {
        if (isCurrentRequest) {
          setProducts(publicProducts.slice(0, 6));
        }
      })
      .catch((loadError: unknown) => {
        console.error("Erro ao carregar produtos em destaque:", loadError);
        if (isCurrentRequest) setError(true);
      })
      .finally(() => {
        if (isCurrentRequest) setIsLoading(false);
      });

    return () => {
      isCurrentRequest = false;
    };
  }, [reloadKey]);

  return (
    <section
      id="featured-products"
      className="bg-[#e7e4e4] py-14 sm:py-16 lg:py-20"
      aria-labelledby="featured-products-title"
    >
      <Container>
        <div className="relative mx-auto max-w-3xl border-4 border-[#593006] px-3 pb-7 pt-12 sm:px-6 sm:pb-9 sm:pt-14 lg:max-w-5xl">
          <h2
            id="featured-products-title"
            className="absolute left-1/2 top-0 -translate-x-1/2 -translate-y-1/2 whitespace-nowrap bg-[#593006] px-7 py-2 text-center text-lg font-semibold text-white sm:px-10 sm:text-xl"
          >
            Best-Sellers
          </h2>

          <div className="mx-auto w-full max-w-2xl [&>div]:!grid-cols-2 [&>div]:!gap-x-3 [&>div]:!gap-y-8 sm:[&>div]:!gap-x-5 sm:[&>div]:!gap-y-10">
            {isLoading && (
              <p role="status" className="py-8 text-center text-sm text-[#7c726b]">
                Carregando produtos...
              </p>
            )}

            {!isLoading && error && (
              <div role="alert" className="space-y-3 py-8 text-center text-sm text-[#7c726b]">
                <p>Não foi possível carregar os produtos. Tente novamente.</p>
                <button
                  type="button"
                  onClick={() => {
                    setIsLoading(true);
                    setError(false);
                    setReloadKey((key) => key + 1);
                  }}
                  className="border border-[#593006] px-4 py-2 font-bold text-[#593006] transition-colors hover:bg-[#593006] hover:text-white"
                >
                  Tentar novamente
                </button>
              </div>
            )}

            {!isLoading && !error && products.length === 0 && (
              <p className="py-8 text-center text-sm text-[#7c726b]">
                Nenhum produto disponível no momento.
              </p>
            )}

            {!isLoading && !error && products.length > 0 && (
              <ProductGrid products={products} />
            )}
          </div>
        </div>
      </Container>
    </section>
  );
}

export function PromoBannersSection() {

  return null;

}

export function NewsletterSection() {

  return null;

}

export function HomeCta() {

  return null;

}