# ADR-004 - Server State com TanStack Query

## Status

Aceita.

## Contexto

Produtos, categorias, pedidos e dados de cliente são estados vindos da SGLS API. Esses dados precisam de cache, invalidação e controle de revalidação no cliente sem introduzir estado global indiscriminado.

## Decisão

O estado de servidor no cliente será gerenciado com TanStack Query. Um `QueryClient` único será criado no provider de cliente e exposto pela composição de providers da aplicação.

## Consequências

- Hooks de consulta devem ficar próximos das features.
- Componentes React não devem chamar HTTP diretamente.
- Estado local de UI continuará usando APIs nativas do React quando necessário.
