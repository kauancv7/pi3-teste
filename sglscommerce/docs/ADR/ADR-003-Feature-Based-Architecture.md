# ADR-003 - Feature-Based Architecture

## Status

Aceita.

## Contexto

O SGLS Commerce é a aplicação Next.js responsável pela experiência do cliente final em uma arquitetura headless. O Commerce deve consumir a SGLS API e não deve acessar diretamente o PostgreSQL.

## Decisão

A aplicação será organizada por features para manter componentes, hooks, schemas, serviços e tipos próximos ao domínio funcional que representam. A pasta `app/` ficará focada em rotas, layouts, metadata, loading, error boundaries, not-found e composição.

A camada de comunicação seguirá o fluxo:

```text
Component
  ↓
Hook
  ↓
Service
  ↓
API Client
  ↓
SGLS API
```

## Consequências

- Regras específicas de negócio não devem ser colocadas em páginas do App Router.
- Componentes globais devem ficar restritos a elementos reutilizáveis de layout, UI ou uso comum.
- Services existentes serão preservados e evoluídos sem refatorações destrutivas.
