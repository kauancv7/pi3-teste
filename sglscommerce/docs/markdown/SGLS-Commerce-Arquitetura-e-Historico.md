# SGLS Commerce — Documento de Arquitetura e Histórico do Projeto

> Documento consolidado do que foi definido e construído até o momento no SGLS Commerce.
>
> **Objetivo:** manter uma fonte de referência para arquitetura, decisões técnicas, estrutura do projeto, estado atual e próximos passos.

---

# 1. Visão do Projeto

O **SGLS Commerce** é o frontend de e-commerce destinado aos clientes finais, integrado ao BackOffice existente da empresa.

O ecossistema será composto por três aplicações:

```text
SGLS
├── sglsbackend
└── sglscommerce
```

Arquitetura geral:

```text
Cliente
   ↓
SGLS Commerce
   ↓
SGLS API
   ↓
PostgreSQL
   ↑
SGLS BackOffice
```

O sistema utiliza **um único banco PostgreSQL**.

Não haverá:

- segundo banco para o Commerce;
- sincronização entre bancos;
- cópia independente de produtos;
- cópia independente de estoque;
- mecanismo de sincronização entre Commerce e BackOffice.

O Commerce e o BackOffice utilizarão a mesma fonte de dados através da API.

---

# 2. Sistemas do Ecossistema

## SGLS API

Backend responsável por expor os recursos necessários ao Commerce e às demais aplicações.

Tecnologias:

- ASP.NET 8
- Clean Architecture
- Entity Framework Core
- PostgreSQL

A API será a autoridade para regras de negócio e dados comerciais.

---

## SGLS BackOffice

Sistema já existente e responsável pela operação interna da empresa.

Módulos existentes:

- Dashboard
- Produtos
- Categorias
- Estoque
- Pedidos
- Funcionários
- Relatórios
- Configurações

Também deverá ser responsável por:

- Produtos
- Categorias
- Estoque
- Pedidos
- Clientes
- Configurações
- CMS futuro

Toda venda realizada pelo Commerce deverá aparecer automaticamente no módulo de Pedidos do BackOffice.

O estoque será atualizado pela API.

---

## SGLS Commerce

Frontend público do e-commerce.

Tecnologias planejadas:

- Next.js
- React
- TypeScript
- Tailwind CSS
- App Router
- Shadcn/UI
- Axios
- TanStack Query
- React Hook Form
- Zod
- Lucide Icons
- Framer Motion

O projeto foi criado sem a pasta `src`.

Essa decisão foi mantida:

```text
sglscommerce/
├── app/
├── components/
├── features/
├── providers/
├── services/
└── ...
```

---

# 3. Objetivos Arquiteturais

O desenvolvimento não deve ser apenas orientado a código.

As decisões devem considerar:

- arquitetura;
- boas práticas;
- Clean Code;
- SOLID;
- componentização;
- manutenção;
- escalabilidade;
- acessibilidade;
- performance;
- separação de responsabilidades;
- experiência do usuário;
- evolução futura para SaaS.

O projeto deverá evitar decisões que criem acoplamento desnecessário.

---

# 4. Estratégia de Arquitetura Frontend

Foi adotada uma arquitetura **feature-based**.

Conceito:

```text
features/
├── home/
├── products/
├── cart/
└── ...
```

As rotas em `app/` devem permanecer relativamente finas, funcionando principalmente como camada de composição e roteamento.

Exemplo:

```text
app/(store)/products/page.tsx
        ↓
features/products/components/products-page.tsx
```

A regra geral é:

```text
app/
  → composição/roteamento

features/
  → domínio e implementação da feature

components/
  → componentes compartilhados

services/
  → comunicação externa/API

providers/
  → composição de providers globais
```

---

# 5. Server Components e Client Components

A estratégia adotada é utilizar **Server Components por padrão**.

Client Components somente devem existir quando realmente houver necessidade de:

- estado;
- interação client-side;
- APIs do navegador;
- eventos interativos;
- contexto client-side.

Não transformar uma página inteira em Client Component quando somente uma pequena parte precisa de interatividade.

Essa decisão é especialmente importante para:

- Header;
- ProductDetail;
- Carrinho;
- Checkout.

---

# 6. Design System

O projeto possui uma fundação inicial de Design System.

Primitives existentes incluem:

- Badge
- Card
- CardContent
- Input
- Typography
- Button

Também foi adotado um `Container` compartilhado.

A regra é reutilizar os tokens e primitives existentes antes de criar estilos ou componentes paralelos.

O Design System deverá evoluir de forma própria.

A loja William Modas será utilizada somente como referência de UX/UI.

Não queremos copiar sua identidade visual.

---

# 7. Container

Foi criado:

```text
components/layout/Container.tsx
```

Responsabilidades:

- largura máxima consistente;
- centralização;
- padding responsivo;
- reutilização em toda a aplicação.

O Container é estrutural.

Não possui:

- lógica de API;
- autenticação;
- estado;
- carrinho;
- domínio de produto.

A decisão foi manter uma única base horizontal, evitando múltiplas variantes prematuras.

---

# 8. Layout Global

O Root Layout mantém:

- metadata;
- `<html>`;
- `<body>`;
- AppProviders.

Foi adicionada a estrutura semântica global:

```html
<main>
```

O layout raiz continua Server Component.

A semântica principal da aplicação foi centralizada para evitar múltiplos `<main>` aninhados.

---

# 9. Store Layout

A estrutura atual da experiência de loja foi organizada para:

```text
TopBar
   ↓
Header
   ↓
Navbar
   ↓
Conteúdo
   ↓
Footer
```

O `app/(store)/layout.tsx` funciona como composição dessa experiência.

---

# 10. Providers

Foi criado o `QueryProvider` como Client Component.

Responsabilidades:

- criar o `QueryClient`;
- utilizar `QueryClientProvider`;
- preservar uma instância estável do QueryClient durante a montagem.

A criação utiliza `useState` para evitar recriação do client a cada render.

Também existe:

```text
providers/app-providers.tsx
```

O `AppProviders` é o ponto de composição dos providers client-side.

Estrutura atual:

```text
AppProviders
   ↓
QueryProvider
   ↓
children
```

Essa estrutura deixa espaço para futuros providers, mas evita criar providers artificiais antes de existir necessidade.

---

# 11. API Client

O Axios foi centralizado em:

```text
services/api.ts
```

O client está tipado utilizando:

- `AxiosInstance`
- `AxiosError`

Não foi utilizado `any`.

Até o momento, o API Client permanece preparado, mas o catálogo ainda utiliza mocks.

A integração real com a SGLS API será feita posteriormente, após definição dos contratos definitivos.

---

# 12. TopBar

Foi criado:

```text
components/layout/TopBar.tsx
```

Características:

- Server Component;
- sem hooks;
- sem estado;
- sem `"use client"`;
- usa Container;
- utiliza tokens existentes do Design System.

A TopBar foi pensada inicialmente como elemento discreto da experiência visual.

No desenvolvimento posterior, foi integrada à composição do Store Layout.

---

# 13. Header

Foi criado o Header canônico:

```text
components/layout/Header.tsx
```

O Header possui:

- área de logo;
- busca;
- ação de conta;
- ação de carrinho.

Responsividade:

### Mobile

```text
Logo + ações
      ↓
Busca
```

### Desktop

```text
Logo | Busca | Ações
```

A busca utiliza o primitive `Input` existente.

Ícones de Search, User e Shopping Cart foram implementados diretamente como SVG porque a instalação de `lucide-react` foi bloqueada pelo registry do ambiente.

Não foi criada dependência artificial.

---

# 14. Navbar

Foi criado:

```text
components/layout/Navbar.tsx
```

A Navbar:

- é Server Component;
- utiliza `next/link`;
- utiliza `Container`;
- utiliza `<nav>`;
- utiliza `<ul>/<li>`;
- possui foco visível;
- possui navegação semântica.

Inicialmente existia somente:

```text
Início → /
```

Posteriormente foi adicionado:

```text
Produtos → /products
```

A navegação foi mantida simples.

No mobile, foi utilizada navegação horizontal com overflow para evitar hacks e evitar criar prematuramente um mega menu ou drawer.

---

# 15. Footer

O projeto já possuía:

```text
components/layout/site-footer.tsx
```

Ele foi reutilizado na experiência da loja.

O conteúdo foi ajustado para identificar corretamente o storefront demonstrativo.

---

# 16. Home

A Home deixou de ser um placeholder técnico.

A rota:

```text
app/(store)/page.tsx
```

permanece enxuta e atua como composição.

A implementação visual foi colocada em:

```text
features/home/components/home-sections.tsx
```

Dados temporários:

```text
features/home/data/mock-storefront.ts
```

A Home possui:

- Hero;
- categorias;
- produtos em destaque;
- CTA.

Foram reutilizados:

- Container;
- Badge;
- Card;
- CardContent;
- Typography.

---

# 17. Dados Mockados

Os dados mockados foram mantidos claramente separados dos contratos definitivos da API.

Produtos foram posteriormente retirados dos mocks específicos da Home e transferidos para o domínio Products.

A Home mantém somente os dados específicos da Home.

Essa separação evita múltiplas fontes locais para o mesmo domínio.

---

# 18. Domínio Products

Foi criado o tipo:

```text
features/products/types/product.ts
```

O tipo `Product` é um modelo de apresentação.

Ele não deve ser tratado automaticamente como o contrato definitivo da API.

A integração futura poderá adaptar o contrato da API para o modelo utilizado pela UI.

---

# 19. Catálogo de Produtos

Foi criada a rota:

```text
/products
```

Arquivo:

```text
app/(store)/products/page.tsx
```

A rota é Server Component.

Ela delega a apresentação para:

```text
features/products/components/products-page.tsx
```

Os mocks foram centralizados em:

```text
features/products/data/mock-products.ts
```

---

# 20. ProductCard

Foi criado:

```text
features/products/components/product-card.tsx
```

Responsabilidade:

- apresentação do produto;
- categoria;
- nome;
- preço;
- placeholder visual;
- link para detalhe.

O card aponta para:

```text
/products/[slug]
```

O ProductCard é apresentacional e não possui responsabilidade de API ou estado.

---

# 21. ProductGrid

Foi criado:

```text
features/products/components/product-grid.tsx
```

Responsabilidade:

- organizar os produtos em grade;
- responsividade.

Comportamento:

```text
Mobile       → 1 coluna
Tablet       → 2 colunas
Desktop      → 3 colunas
Telas amplas → 4 colunas
```

---

# 22. Product Listing

A página de catálogo apresenta:

- título;
- descrição;
- badge de contexto;
- quantidade de produtos;
- grid;
- preparação visual para filtros/ordenação futuros.

Ainda não existem:

- filtros funcionais;
- busca;
- ordenação;
- paginação;
- API;
- carrinho.

---

# 23. Página de Produto

Foi criada a rota dinâmica:

```text
app/(store)/products/[slug]/page.tsx
```

A apresentação fica em:

```text
features/products/components/product-detail.tsx
```

Fluxo:

```text
/products
   ↓
ProductCard
   ↓
/products/[slug]
```

---

# 24. Busca por Slug

Foi criado um helper dentro dos dados mockados para localizar o produto pelo slug.

A fonte continua sendo:

```text
mockProducts
```

Não existe uma segunda fonte de produtos.

---

# 25. Produto Inexistente

A página dinâmica utiliza o mecanismo nativo do Next.js:

```text
notFound()
```

Assim:

```text
/products/produto-existente
→ produto

/products/slug-inexistente
→ 404
```

A infraestrutura global de `app/not-found.tsx` é reutilizada.

---

# 26. Metadata de Produto

A página dinâmica implementa metadata nativa utilizando:

```text
generateMetadata
```

O título utiliza o nome do produto.

Para produto inexistente, o comportamento permanece compatível com `notFound()`.

Não foi criada biblioteca externa de SEO.

---

# 27. Estrutura do Product Detail

A página apresenta:

- breadcrumb;
- categoria;
- nome;
- preço;
- descrição;
- placeholder de imagem;
- área de ação;
- botão de adicionar ao carrinho.

O botão de carrinho inicialmente permanece desabilitado porque a funcionalidade ainda não existia.

---

# 28. Breadcrumb

Foi implementado breadcrumb semântico:

```text
Início
  >
Produtos
  >
Nome do produto
```

Links reais:

```text
Início → /
Produtos → /products
```

O produto atual não precisa ser link.

---

# 29. Acessibilidade

Foram adotadas práticas como:

- headings semânticos;
- `h1` único;
- `section`;
- `article`;
- `nav`;
- `footer`;
- breadcrumb semântico;
- foco visível;
- links semânticos;
- placeholders acessíveis;
- `aria-labelledby` quando necessário;
- evitar ARIA desnecessário.

Acessibilidade deve continuar sendo considerada em todas as próximas features.

---

# 30. Responsividade

A abordagem é mobile-first.

Exemplo do Product Detail:

### Mobile

```text
Imagem
  ↓
Informações
  ↓
Ação
```

### Desktop

```text
Imagem | Informações
```

A Home, catálogo e layout também foram estruturados com breakpoints responsivos.

---

# 31. Decisões de API

Até o momento:

**não existe integração real do catálogo com a SGLS API.**

Isso foi intencional.

Não foram criados:

- contratos fictícios;
- hooks de API;
- serviços específicos de produtos;
- endpoints inventados;
- chamadas Axios para mocks;
- TanStack Query para dados locais.

A integração será feita quando o contrato definitivo da API estiver definido.

---

# 32. Separação de Server State e Client State

Uma decisão arquitetural importante já foi estabelecida:

```text
TanStack Query
      ↓
Server State
      ↓
SGLS API
```

Enquanto:

```text
React Context + useReducer
      ↓
Client State
      ↓
Carrinho
```

Essa separação será importante para evitar utilizar TanStack Query como mecanismo de estado global do carrinho.

---

# 33. Carrinho — Arquitetura Definida

O próximo domínio importante é o carrinho.

A auditoria arquitetural definiu:

```text
React Context + useReducer
```

como solução inicial.

Não será utilizado Zustand neste momento.

Motivos:

- o problema é relativamente pequeno;
- React já fornece as primitivas necessárias;
- evita adicionar dependência;
- facilita entender o fluxo de estado;
- mantém o domínio explícito;
- possui boa testabilidade.

Zustand poderá ser reconsiderado no futuro caso a complexidade realmente justifique.

---

# 34. Carrinho — Modelo

A direção aprovada é separar o modelo de catálogo do modelo do carrinho.

Não queremos acoplar:

```text
CartItem
   ↓
Product inteiro
```

Preferimos um modelo comercial próprio.

Conceitualmente:

```text
Cart
├── schemaVersion
├── currency
└── items[]
```

E:

```text
CartItem
├── key
├── productId
├── variantId?
├── sku?
├── quantity
├── unitPriceCents
└── productSnapshot
```

---

# 35. CartProductInput

Foi definido que deverá existir um modelo intermediário:

```text
CartProductInput
```

Sua responsabilidade será representar os dados mínimos necessários para transformar um produto do catálogo em um item do carrinho.

Isso evita acoplamento direto entre:

```text
Product
```

e:

```text
CartItem
```

---

# 36. Identidade do Item

A primeira versão utilizará uma chave conceitual:

```text
productId:default
```

O modelo deve permitir evolução futura para:

- variantId;
- SKU;
- tamanho;
- cor;
- outras características vendáveis.

Variantes ainda NÃO serão implementadas.

---

# 37. Preços do Carrinho

O carrinho trabalhará internamente com centavos:

```text
unitPriceCents: number
```

Exemplo:

```text
R$ 249,90
     ↓
24990
```

Isso evita problemas comuns de cálculos monetários usando valores decimais.

O preço do carrinho é uma informação para a experiência local.

A autoridade final continuará sendo o backend.

---

# 38. Autoridade do Backend

O frontend NÃO será autoridade final para:

- preço;
- estoque;
- descontos;
- frete;
- impostos;
- total final;
- criação do pedido.

O frontend pode calcular valores para UX.

No checkout, a SGLS API deverá validar novamente os dados.

Fluxo futuro:

```text
Commerce
   ↓
Checkout
   ↓
SGLS API
   ↓
validação
   ↓
Pedido
```

---

# 39. Estado Derivado do Carrinho

Não devemos armazenar redundantemente:

```text
totalItems
subtotal
total
```

se esses valores puderem ser derivados de:

```text
items
```

A direção aprovada é utilizar funções puras para calcular:

```text
getTotalItems()
getSubtotalCents()
```

O total inicial será equivalente ao subtotal.

---

# 40. Reducer do Carrinho

A fundação planejada utilizará ações como:

```text
addItem
removeItem
setQuantity
clearCart
```

Comportamentos:

### addItem

Se o item já existir:

```text
quantity += nova quantidade
```

Caso contrário:

```text
novo item
```

### removeItem

Remove pelo `key`.

### setQuantity

Atualiza a quantidade.

Quantidade menor ou igual a zero remove o item.

### clearCart

Retorna carrinho vazio preservando:

- schemaVersion;
- currency.

---

# 41. Persistência

A primeira implementação de persistência planejada é:

```text
localStorage
```

Motivos:

- simples;
- suficiente para carrinho inicial;
- não exige backend;
- mantém o usuário após refresh.

A persistência deverá considerar:

- SSR;
- hidratação;
- serialização;
- versão do schema.

O localStorage ainda NÃO foi implementado na fundação do domínio.

---

# 42. SaaS Futuro

O projeto poderá futuramente se tornar uma plataforma SaaS para várias lojas.

Não implementaremos multi-tenancy agora.

Porém, devemos evitar decisões que dificultem essa evolução.

Exemplo de ideia futura:

```text
sgls-commerce:cart:v1:<storeId>
```

Essa estratégia não deve ser implementada prematuramente.

A arquitetura deve apenas permanecer compatível com essa evolução.

---

# 43. Estrutura Planejada do Carrinho

A estrutura inicial aprovada é deliberadamente pequena:

```text
features/cart/
├── types/
│   └── cart.ts
└── state/
    └── cart-reducer.ts
```

Não queremos criar prematuramente:

```text
context/
persistence/
components/
utils/
selectors/
services/
```

sem necessidade real.

A arquitetura deve crescer conforme a complexidade real do domínio.

---

# 44. Próximas Etapas do Carrinho

A sequência planejada é:

```text
1. Fundação do domínio
       ↓
2. CartProvider
       ↓
3. Persistência local
       ↓
4. AddToCart
       ↓
5. Contador no Header
       ↓
6. Página /cart
       ↓
7. Estado vazio
       ↓
8. Quantidades
       ↓
9. Remoção
       ↓
10. Preparação para Checkout
```

---

# 45. Checkout Futuro

O checkout será uma área posterior.

Ele deverá considerar:

- cliente;
- endereço;
- entrega;
- pagamento;
- estoque;
- descontos;
- preço final;
- criação de pedido.

O frontend não deverá assumir que o estado local do carrinho é suficiente para criar o pedido.

A API deverá validar novamente.

---

# 46. Roadmap Inicial Original

O roadmap inicialmente definido foi:

```text
Sprint 0
- Organização do projeto
- Design System
- Layout Base

Sprint 1
- TopBar
- Header
- Navbar
- Footer
- Container

Sprint 2
- Home

Sprint 3
- Produtos

Sprint 4
- Página do Produto

Sprint 5
- Carrinho

Sprint 6
- Checkout

Sprint 7
- Minha Conta

Sprint 8
- Integração completa com a API

Sprint 9
- Testes

Sprint 10
- Docker, Nginx e Deploy
```

Posteriormente foi tomada a decisão de **abandonar a divisão rígida por Sprints** para agilizar o desenvolvimento do frontend.

O roadmap continua servindo como referência, mas o trabalho passou a ser conduzido por incrementos funcionais.

---

# 47. Fluxo Atual do Frontend

Neste momento, a experiência construída é:

```text
                    SGLS COMMERCE

                         Home
                          │
              ┌───────────┴───────────┐
              │                       │
              ▼                       ▼
          Categorias              Produtos
                                      │
                                      ▼
                               Product Listing
                                      │
                                      ▼
                               Product Card
                                      │
                                      ▼
                              Product Detail
                                      │
                                      ▼
                              Add to Cart
                                  (próximo)
```

---

# 48. Estrutura Atual Conceitual

```text
sglscommerce/
│
├── app/
│   ├── (store)/
│   │   ├── layout.tsx
│   │   ├── page.tsx
│   │   └── products/
│   │       ├── page.tsx
│   │       └── [slug]/
│   │           └── page.tsx
│   │
│   ├── layout.tsx
│   └── not-found.tsx
│
├── components/
│   ├── layout/
│   │   ├── Container.tsx
│   │   ├── Header.tsx
│   │   ├── Navbar.tsx
│   │   ├── TopBar.tsx
│   │   └── site-footer.tsx
│   │
│   └── ui/
│       ├── Badge
│       ├── Button
│       ├── Card
│       ├── Input
│       └── Typography
│
├── features/
│   ├── home/
│   │   ├── components/
│   │   └── data/
│   │
│   ├── products/
│   │   ├── components/
│   │   ├── data/
│   │   └── types/
│   │
│   └── cart/
│       ├── state/
│       └── types/
│
├── providers/
│   ├── app-providers.tsx
│   └── query-provider.tsx
│
└── services/
    └── api.ts
```

> A estrutura acima representa a arquitetura consolidada até aqui. O filesystem real continua sendo a fonte de verdade durante a implementação.

---

# 49. Git e Fluxo de Desenvolvimento

O desenvolvimento utilizou branches e merges com a `main`.

Em determinado momento foram utilizados commits específicos de implementação e posteriormente Pull Requests foram integrados à `main`.

O histórico registrado mostrou merges como:

```text
Merge pull request #34 ... implementar-navbar...
Merge pull request #33 ... estrutura-visual-do-header
Merge pull request #31 ... estrutura-de-layout...
Merge pull request #30 ... problemas-da-auditoria...
Merge pull request #29 ... design-system...
```

O projeto chegou a um estado em que:

```text
main
↓
origin/main
```

estava sincronizado e o working tree estava limpo.

---

# 50. Commits Relevantes

Alguns commits registrados durante a evolução:

```text
84e2952 feat: add design system foundation

43c55a5 Stabilize frontend provider foundation

447ad92 Add layout foundation container and main

8ad95bb Add responsive commerce header

25d220b Add store navbar

4c6f9d8 feat(store): build storefront home foundation

264df2e feat(products): add product listing foundation

a99f546 feat(products): add product detail page
```

Os hashes podem variar conforme o histórico final após merges.

---

# 51. Validações

Durante as etapas realizadas, foram utilizadas repetidamente:

```bash
npm run lint
npm run typecheck
npm run build
git diff --check
```

Também foram realizados testes HTTP locais, incluindo:

```text
/products
/products/fone-aurora
/products/slug-inexistente
```

Resultados registrados:

```text
/products
→ 200

/products/fone-aurora
→ 200

/products/slug-inexistente
→ 404
```

O build e typecheck estavam passando nas etapas registradas.

---

# 52. Limitações do Ambiente

Durante o desenvolvimento houve algumas limitações de infraestrutura:

## lucide-react

A instalação de:

```text
lucide-react
```

foi bloqueada pelo registry do ambiente com HTTP 403.

Como consequência, os ícones do Header foram implementados como SVG.

---

## GitHub CLI

Em algumas tarefas, não foi possível criar Pull Requests automaticamente porque:

- GitHub CLI não estava autenticado;
- não havia `GH_TOKEN`;
- a ferramenta `make_pr` não estava disponível;
- em alguns ambientes não havia remote Git configurado.

Isso não impediu o desenvolvimento ou commits locais.

---

## Screenshot

A captura visual automática não foi possível em alguns ambientes porque não havia:

- Chromium;
- navegador headless;
- ferramenta local de screenshot.

A validação visual completa ainda deverá ser realizada em ambiente com navegador apropriado.

---

# 53. Princípios que Devem Continuar

## 1. Não criar abstração sem necessidade

Preferir:

```text
solução simples que funciona
```

a:

```text
framework interno complexo
```

---

## 2. Não antecipar backend

Não inventar:

- endpoints;
- contratos;
- payloads;
- regras de API.

---

## 3. Server Component por padrão

Somente usar Client Component quando necessário.

---

## 4. Features isoladas

Domínios devem permanecer dentro de:

```text
features/<feature>
```

quando fizer sentido.

---

## 5. Design System compartilhado

Evitar estilos duplicados.

---

## 6. Backend como autoridade comercial

Principalmente para:

- estoque;
- preço;
- desconto;
- frete;
- total;
- pedido.

---

## 7. Estado derivado

Não armazenar informações que possam ser calculadas de maneira confiável a partir do estado principal.

---

## 8. Código testável

Domínio e regras devem preferencialmente ser implementados como funções puras sempre que possível.

---

# 54. Estado Atual

O frontend já possui uma fundação funcional com:

- Next.js;
- App Router;
- TypeScript;
- Tailwind;
- Design System inicial;
- layout global;
- Store Layout;
- TopBar;
- Header;
- Navbar;
- Footer;
- Home;
- catálogo;
- ProductCard;
- ProductGrid;
- Product Detail;
- slug handling;
- 404;
- metadata dinâmica;
- QueryProvider;
- API Client;
- arquitetura feature-based.

O próximo grande domínio é:

```text
CARRINHO
```

A fundação arquitetural do carrinho foi definida, mas a implementação ainda deve começar pela camada de domínio.

---

# 55. Próximo Passo

A próxima implementação deve ser:

```text
features/cart/
├── types/
│   └── cart.ts
└── state/
    └── cart-reducer.ts
```

Com:

- `Cart`;
- `CartItem`;
- `CartProductInput`;
- identidade do item;
- preço em centavos;
- `addItem`;
- `removeItem`;
- `setQuantity`;
- `clearCart`;
- `getTotalItems`;
- `getSubtotalCents`;
- testes unitários.

Depois:

```text
Cart Domain
    ↓
CartProvider
    ↓
localStorage
    ↓
AddToCart
    ↓
Header Counter
    ↓
/cart
```

---

# 56. Visão de Longo Prazo

A visão do SGLS Commerce é evoluir de um frontend de uma loja para uma plataforma de e-commerce potencialmente SaaS.

A arquitetura deve permitir, no futuro:

```text
                    SGLS Platform
                         │
             ┌───────────┼───────────┐
             │           │           │
           Loja A      Loja B      Loja C
             │           │           │
          Commerce    Commerce    Commerce
             │           │           │
             └───────────┼───────────┘
                         │
                      SGLS API
                         │
                    PostgreSQL
                         │
                    BackOffice
```

Isso não significa implementar multi-tenancy agora.

Significa apenas evitar decisões arquiteturais que tornem essa evolução desnecessariamente difícil.

---

# 57. Regra de Ouro do Projeto

O objetivo não é simplesmente produzir telas.

O objetivo é construir um software profissional.

Cada nova implementação deve responder:

1. Qual problema estamos resolvendo?
2. Por que essa solução?
3. Existe uma solução mais simples?
4. Qual o custo de manutenção?
5. Isso respeita a arquitetura atual?
6. Isso cria acoplamento?
7. Isso dificulta uma futura integração com a API?
8. Isso continua compatível com Server Components?
9. Isso facilita testes?
10. Isso ajuda ou prejudica uma futura evolução SaaS?

A velocidade deve vir de **boas decisões e incrementos pequenos**, e não de acumular código sem estrutura.

---

# 58. Documento Vivo

Este documento deve ser atualizado quando houver uma decisão arquitetural significativa.

Não é necessário registrar cada alteração trivial.

Devem ser registrados principalmente:

- mudanças de arquitetura;
- novos domínios;
- decisões sobre estado;
- contratos importantes;
- decisões de infraestrutura;
- decisões que afetem escalabilidade;
- mudanças na estratégia de integração com API.

---

**SGLS Commerce — Documento consolidado de arquitetura e desenvolvimento**

Status atual:

```text
Layout              ✅
Design System       ✅ Fundação
Home                ✅
Products Listing    ✅
Product Detail      ✅
Cart Architecture   ✅ Definida
Cart Implementation ⏳ Próximo
Checkout            ⏳ Futuro
Minha Conta         ⏳ Futuro
API Integration     ⏳ Futuro
Tests               ⏳ Evolução contínua
Docker/Deploy       ⏳ Futuro
SaaS                🔮 Visão futura
```
