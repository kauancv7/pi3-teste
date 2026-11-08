# SGLS Commerce

Frontend da loja SGLS, desenvolvido com Next.js, React e TypeScript.

## Executar localmente

1. Instale as dependências:

   ```bash
   npm install
   ```

2. Configure a URL da API criando `.env.local` a partir de `.env.example`:

   ```env
   NEXT_PUBLIC_API_URL=http://localhost:5150/api
   ```

   O backend deve estar acessível nessa URL. Se estiver rodando em outro host ou
   porta, ajuste `NEXT_PUBLIC_API_URL`.

3. Inicie o frontend:

   ```bash
   npm run dev
   ```

4. Abra [http://localhost:3000](http://localhost:3000).

O frontend não inicia o backend nem o banco de dados. Para ver produtos reais,
o endpoint público deve estar disponível e retornar produtos ativos.

## Sprint 3 — integrações com o backend

### Página inicial

- A seção de categorias consulta `GET /api/produtos/publicos` e agrupa os
  produtos ativos por categoria. A contagem exibida corresponde aos produtos
  retornados para cada categoria.
- A seção “Mais Vendidos & Favoritos” também usa produtos retornados por essa
  rota, em vez da lista mockada.
- As categorias da Home são derivadas das categorias dos produtos; não há uma
  chamada a um endpoint separado de categorias.
- Banners e informações de benefícios da loja continuam sendo conteúdo de
  apresentação estático.

### Busca

- A busca do cabeçalho direciona para `/busca?q=<termo>`.
- A página carrega produtos de `GET /api/produtos/publicos` e filtra por nome
  e categoria, ignorando diferenças de maiúsculas e acentuação.
- A tela apresenta estados de carregamento, erro e nenhum resultado.
- Sem parâmetro `q`, exibe todos os produtos retornados pela API.

### Login do cliente

- Há uma página de login em `/login`, além do formulário usado pelo painel
  “Minha Conta”.
- O formulário envia email e senha a
  `POST /api/v1/auth/cliente/login`.
- Em caso de sucesso, o token e sua expiração são guardados em
  `sessionStorage`; erros de autenticação e estado de envio são exibidos no
  formulário.
- O login depende de um cliente cadastrado e da tabela `Clientes` existir no
  banco. No ambiente de desenvolvimento usado durante a Sprint 3, o endpoint
  retornou erro 500 informando que a relação `Clientes` não existia. Portanto,
  a integração do frontend está implementada, mas o login bem-sucedido precisa
  ser validado após o backend/banco ser corrigido.

### Produtos e detalhes

- Um serviço compartilhado converte a resposta da API pública para o modelo de
  apresentação usado pelo catálogo, Home, busca e detalhes.
- A página de detalhes em `/products/[id]` localiza o produto na lista pública,
  sem usar os produtos mockados. Exibe nome, preço, categoria, descrição e
  estoque, com estados de carregamento, erro e produto não encontrado.
- O botão de adicionar ao carrinho é desativado quando o estoque informado pela
  API é zero.
- O endpoint público fornece produtos, mas não dados de vendas ou ordenação por
  quantidade vendida. Assim, os primeiros produtos ativos retornados são
  exibidos na seção intitulada “Mais Vendidos & Favoritos”; não se trata de um
  ranking real de vendas.

## Testes e validação

Com o backend acessível e produtos ativos cadastrados:

- Abra a página inicial e confira as categorias e os produtos em destaque.
- Pesquise pelo cabeçalho ou acesse `/busca?q=produto` e confira os resultados.
- Abra um produto e confira os dados na página de detalhes.
- Teste `/login` com credenciais de um cliente cadastrado. Uma conta Admin ou
  Vendedor é uma conta interna e não substitui um cliente.

Comandos do projeto:

```bash
npm test
npm run lint
npm run typecheck
```

Na validação da Sprint 3, os 9 testes unitários existentes passaram. O lint dos
arquivos relacionados às integrações passou sem erros, com avisos existentes
sobre uso de `<img>`. O typecheck permaneceu bloqueado por uma referência
gerada ausente em `.next` e pela dependência `lucide-react`, que não consta
instalada no projeto.
