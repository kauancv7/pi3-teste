import Link from "next/link";

import { Container } from "@/components/layout/Container";
import { Typography } from "@/components/ui/typography";

export default function NotFound() {
  return (
    <Container className="flex flex-1 flex-col justify-center py-20">
      <Typography as="p" variant="label" className="uppercase tracking-[0.2em] text-primary">
        404
      </Typography>
      <Typography variant="h1" className="mt-3">
        Página não encontrada
      </Typography>
      <Typography className="mt-4 max-w-xl">
        A rota solicitada não existe no SGLS Commerce.
      </Typography>
      <Link
        href="/"
        className="mt-8 inline-flex h-11 items-center justify-center rounded-full bg-primary px-5 text-sm font-semibold text-primary-foreground transition-colors hover:bg-primary-hover"
      >
        Voltar para a página inicial
      </Link>
    </Container>
  );
}
