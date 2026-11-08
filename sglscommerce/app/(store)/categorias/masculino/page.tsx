
"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { api } from "@/services/api";
import ProdutoImagem from "@/components/products/ProdutoImagem";

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

export default function MasculinoPage() {
  const [produtos, setProdutos] = useState<Produto[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState("");

  useEffect(() => {
    async function carregarProdutos() {
      try {
        const resposta = await api.get<Produto[]>("/produtos/publicos");

        const produtosMasculinos = resposta.data.filter(
          (produto) =>
            produto.ativo &&
            produto.categoriaNome?.trim().toLowerCase() === "masculino"
        );

        setProdutos(produtosMasculinos);
      } catch (error) {
        console.error("Erro ao buscar produtos:", error);
        setErro("Não foi possível carregar os produtos. Tente novamente.");
      } finally {
        setCarregando(false);
      }
    }

    carregarProdutos();
  }, []);

  return (
    <main className="min-h-screen bg-[#f8f9fa] text-[#231c19]">
      {/* Cabeçalho da categoria */}
      <section className="border-b border-[#e2ddd5] bg-white">
        <div className="mx-auto max-w-7xl px-4 py-5 sm:px-6 lg:px-8">
          <nav
            aria-label="Navegação estrutural"
            className="flex items-center gap-2 text-[11px] font-medium uppercase tracking-wider text-[#7c726b]"
          >
            <Link href="/" className="transition-colors hover:text-[#8b4513]">
              Início
            </Link>
            <span className="text-[#c6b9ad]">/</span>
            <span className="text-[#231c19]">Masculino</span>
          </nav>
        </div>
      </section>

      {/* Título e descrição */}
      <section className="mx-auto max-w-7xl px-4 pb-8 pt-10 sm:px-6 sm:pb-10 sm:pt-14 lg:px-8">
        <div className="flex flex-col justify-between gap-5 border-b border-[#e2ddd5] pb-6 sm:flex-row sm:items-end">
          <div>
            <p className="text-[11px] font-black uppercase tracking-[0.25em] text-[#8b5e3c]">
              Lojas Stuart · Coleção
            </p>

            <h1 className="mt-2 text-3xl font-black uppercase tracking-tight text-[#231c19] sm:text-4xl lg:text-5xl">
              Masculino
            </h1>

            <p className="mt-3 max-w-xl text-sm leading-6 text-[#7c726b] sm:text-base">
              Encontre peças para compor seu estilo. Confira nossa coleção
              masculina e descubra seus próximos favoritos.
            </p>
          </div>

          {!carregando && !erro && (
            <p className="shrink-0 text-xs font-bold uppercase tracking-wider text-[#7c726b]">
              {produtos.length}{" "}
              {produtos.length === 1
                ? "produto encontrado"
                : "produtos encontrados"}
            </p>
          )}
        </div>
      </section>

      {/* Listagem dos produtos */}
      <section className="mx-auto max-w-7xl px-4 pb-16 sm:px-6 lg:px-8">
        {carregando && (
          <div className="grid grid-cols-2 gap-x-4 gap-y-8 sm:grid-cols-3 lg:grid-cols-4 lg:gap-x-6">
            {Array.from({ length: 8 }).map((_, index) => (
              <div key={index} className="animate-pulse">
                <div className="aspect-[3/4] bg-[#eae6e0]" />
                <div className="mt-4 h-3 w-2/3 bg-[#eae6e0]" />
                <div className="mt-3 h-4 w-1/2 bg-[#eae6e0]" />
                <div className="mt-3 h-3 w-1/3 bg-[#eae6e0]" />
              </div>
            ))}
          </div>
        )}

        {!carregando && erro && (
          <div className="border border-[#e2ddd5] bg-white px-5 py-16 text-center">
            <p className="text-lg font-bold text-[#231c19]">
              Não foi possível carregar a coleção
            </p>
            <p className="mt-2 text-sm text-[#7c726b]">{erro}</p>

            <button
              onClick={() => window.location.reload()}
              className="mt-6 bg-[#2d221e] px-6 py-3 text-xs font-black uppercase tracking-wider text-white transition-colors hover:bg-[#8b4513]"
            >
              Tentar novamente
            </button>
          </div>
        )}

        {!carregando && !erro && produtos.length === 0 && (
          <div className="border border-[#e2ddd5] bg-white px-5 py-16 text-center">
            <p className="text-lg font-bold uppercase text-[#231c19]">
              Nenhum produto encontrado
            </p>
            <p className="mt-2 text-sm text-[#7c726b]">
              Ainda não temos produtos disponíveis nesta coleção.
            </p>
            <Link
              href="/"
              className="mt-6 inline-flex bg-[#2d221e] px-6 py-3 text-xs font-black uppercase tracking-wider text-white transition-colors hover:bg-[#8b4513]"
            >
              Voltar à página inicial
            </Link>
          </div>
        )}

        {!carregando && !erro && produtos.length > 0 && (
          <div className="grid grid-cols-2 gap-x-4 gap-y-9 sm:grid-cols-3 lg:grid-cols-4 lg:gap-x-6 lg:gap-y-12">
            {produtos.map((produto) => (
              <article key={produto.id} className="group min-w-0">
                {/* Imagem */}
                <div className="relative overflow-hidden border border-[#e2ddd5] bg-[#f3f1ed]">
                  <ProdutoImagem
                    imagemId={produto.imagemId}
                    nome={produto.nome}
                  />

                  <span className="absolute left-2 top-2 bg-[#2d221e] px-2 py-1 text-[9px] font-black uppercase tracking-wider text-white sm:left-3 sm:top-3">
                    Lojas Stuart
                  </span>

                  {produto.estoque <= 0 && (
                    <div className="absolute inset-0 flex items-center justify-center bg-white/70">
                      <span className="bg-[#231c19] px-3 py-2 text-[10px] font-black uppercase tracking-wider text-white">
                        Esgotado
                      </span>
                    </div>
                  )}
                </div>

                {/* Informações */}
                <div className="border-b border-[#e2ddd5] px-1 py-4 sm:px-2">
                  <p className="text-[9px] font-bold uppercase tracking-[0.18em] text-[#8b5e3c]">
                    Coleção masculina
                  </p>

                  <h2 className="mt-2 line-clamp-2 min-h-10 text-sm font-bold uppercase leading-5 text-[#231c19] transition-colors group-hover:text-[#8b4513] sm:text-base">
                    {produto.nome}
                  </h2>

                  {produto.descricao && (
                    <p className="mt-1 line-clamp-2 min-h-8 text-xs leading-4 text-[#7c726b]">
                      {produto.descricao}
                    </p>
                  )}

                  <div className="mt-4 flex flex-wrap items-end justify-between gap-2">
                    <p className="text-base font-black tracking-tight text-[#231c19] sm:text-lg">
                      {produto.preco.toLocaleString("pt-BR", {
                        style: "currency",
                        currency: "BRL",
                      })}
                    </p>

                    <span
                      className={`text-[9px] font-bold uppercase tracking-wide ${
                        produto.estoque > 0
                          ? "text-[#68754b]"
                          : "text-red-700"
                      }`}
                    >
                      {produto.estoque > 0 ? "Disponível" : "Indisponível"}
                    </span>
                  </div>
                </div>
              </article>
            ))}
          </div>
        )}
      </section>
    </main>
  );
}
