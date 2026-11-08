
"use client";

import { useEffect, useState } from "react";
import Image from "next/image";
import { api } from "@/services/api";

interface ProdutoImagemProps {
  imagemId?: string;
  nome: string;
}

export default function ProdutoImagem({
  imagemId,
  nome,
}: ProdutoImagemProps) {
  const [imagemUrl, setImagemUrl] = useState<string | null>(null);

  useEffect(() => {
    if (!imagemId) return;

    let urlTemporaria: string | null = null;
    let cancelado = false;

    async function carregarImagem() {
      try {
        const resposta = await api.get(
          `/images/${imagemId}/download`,
          { responseType: "blob" }
        );

        if (cancelado) return;

        urlTemporaria = URL.createObjectURL(resposta.data);
        setImagemUrl(urlTemporaria);
      } catch (error) {
        console.error(`Erro ao carregar imagem de ${nome}:`, error);
      }
    }

    carregarImagem();

    return () => {
      cancelado = true;

      if (urlTemporaria) {
        URL.revokeObjectURL(urlTemporaria);
      }
    };
  }, [imagemId, nome]);

  return (
    <div className="relative flex aspect-[3/4] items-center justify-center overflow-hidden bg-gray-100">
      {imagemUrl ? (
        <Image
          src={imagemUrl}
          alt={nome}
          fill
          unoptimized
          className="object-cover transition-transform duration-300 hover:scale-105"
          sizes="(max-width: 640px) 50vw, (max-width: 1024px) 33vw, 25vw"
        />
      ) : (
        <span className="text-sm text-gray-400">
          Imagem do produto
        </span>
      )}
    </div>
  );
}
