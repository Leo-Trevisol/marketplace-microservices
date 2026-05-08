<h1 align="center">🛒 Marketplace Microservices</h1>

<p align="center">
  Projeto desenvolvido para a disciplina de <strong>Projetos de Sistemas para Web</strong> da graduação em 
  <strong>Análise e Desenvolvimento de Sistemas (ADS)</strong> da <strong>Uniftec</strong>.
</p>

<p align="center">
  Sistema de marketplace baseado em arquitetura de microserviços, com comunicação via APIs REST.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/C%23-.NET-blue?logo=csharp" />
  <img src="https://img.shields.io/badge/ASP.NET-Web%20API-purple?logo=dotnet" />
  <img src="https://img.shields.io/badge/ASP.NET-MVC-blue?logo=dotnet" />
  <img src="https://img.shields.io/badge/PostgreSQL-Database-blue?logo=postgresql" />
  <img src="https://img.shields.io/badge/API-REST-green" />
  <img src="https://img.shields.io/badge/Architecture-Microservices-orange" />
  <img src="https://img.shields.io/badge/Cloud-Azure-lightblue?logo=microsoftazure" />
  <img src="https://img.shields.io/badge/Frontend-Bootstrap-purple?logo=bootstrap" />
</p>

<hr/>

<section>
  <h2>📌 Objetivo</h2>
  <p>
    Desenvolver um marketplace baseado em arquitetura de microserviços, aplicando conceitos de APIs REST, integração entre serviços, persistência de dados e deploy em cloud.
  </p>
</section>

<section>
  <h2>🧱 Arquitetura</h2>
  <p>
    O sistema é composto por um frontend em ASP.NET MVC que consome múltiplos microserviços independentes.
  </p>

<section>
  <h2>⚙️ Tecnologias</h2>

  <h3>Backend</h3>
  <ul>
    <li>C#</li>
    <li>ASP.NET Web API</li>
    <li>PostgreSQL</li>
  </ul>

  <h3>Frontend</h3>
  <ul>
    <li>ASP.NET MVC</li>
    <li>Bootstrap</li>
  </ul>

  <h3>Cloud</h3>
  <ul>
    <li>Microsoft Azure</li>
  </ul>
</section>

<section>
  <h2>💂‍♀️ Entidade - Produto</h2>

  <h3>Produtos</h3>

| Campo                | Tipo      | Descrição                                                  |
| -------------------- | --------- | ---------------------------------------------------------- |
| `Id`                 | `Guid`    | Identificador único                                        |
| `Codigo`             | `string`  | Código único do produto                                    |
| `Nome`               | `string`  | Nome do produto                                            |
| `Preco`              | `decimal` | Preço do produto                                           |
| `QuantidadeEstoque`  | `int`     | Quantidade do produto em estoque                           |
| `EstoqueMinimoVenda` | `int`     | Quantidade de estoque mínimo para disponibilização do produto para venda                                                    |
| `IdCategoria`        | `Guid`    | Identificador único da categoria do produto                |
| `Descricao`          | `string`  | Descrição opcional do produto                              |
| `Disponivel`         | `bool`    | Produto disponível para venda = true. Indisponível = false |

</section>

</section>

<section>
  <h2>🔌 Endpoints</h2>

  <section>
    <h3>POST api/produto/cadastrarProduto - Cadastrar novo produto</h3>
    <h4>Body</h4>
<pre>
{
  "id": "98e82e66-d132-4077-94bd-60dc0f4ffe30",
  "codigo": "PROD-100",
  "nome": "Caneta Azul BIC",
  "preco": 120.90,
  "quantidadeEstoque": 5,
  "estoqueMinimoVenda": 10,
  "idCategoria": "9c8a7b6e-1234-4abc-9def-123456789abc",
  "descricao": "Descrição do produto referente novo teste via postman",
  "disponivel": true
}
</pre>
  <h4>Resposta:</h4>
  <pre>
{
  "sucesso": (true/false),
  "data": {
      "id": "98e82e66-d132-4077-94bd-60dc0f4ffe30",
      "codigo": "PROD-100",
      "nome": "Caneta Azul BIC",
      "preco": 120.90,
      "quantidadeEstoque": 5,
      "estoqueMinimoVenda": 10,
      "idCategoria": "9c8a7b6e-1234-4abc-9def-123456789abc",
      "descricao": "Descrição do produto referente novo teste via postman",
      "disponivel": true
  }
  "message": (Mensagem de retorno)
}
</pre>
  </section>
  
  <section>
    <h3>PUT api/produto/atualizarProduto - Atualizar produto existente</h3>
    <h4>Body</h4>
<pre>
{
  "id": "98e82e66-d132-4077-94bd-60dc0f4ffe30",
  "codigo": "PROD-100",
  "nome": "Caneta Vermelha BIC",
  "preco": 130.90,
  "quantidadeEstoque": 10,
  "estoqueMinimoVenda": 10,
  "idCategoria": "9c8a7b6e-1234-4abc-9def-123456789abc",
  "descricao": "Descrição do produto referente novo teste via postman",
  "disponivel": true
}
</pre>
  <h4>Resposta:</h4>
  <pre>
  {
    "sucesso": (true/false),
    "data": {
        "id": "98e82e66-d132-4077-94bd-60dc0f4ffe30",
        "codigo": "PROD-100",
        "nome": "Caneta Vermelha BIC",
        "preco": 130.90,
        "quantidadeEstoque": 10,
        "estoqueMinimoVenda": 10,
        "idCategoria": "9c8a7b6e-1234-4abc-9def-123456789abc",
        "descricao": "Descrição do produto referente novo teste via postman",
        "disponivel": true
    }
    "message": (Mensagem de retorno)
  }
  </pre>
  </section>
  
  <section>
    <h3>DELETE api/produto/excluirProduto/{codigo} - Deletar produto existente</h3>
    
  <h4>Resposta:</h4>
    <pre>
  {
    "sucesso": (true/false),
    "data": null,
    "message": (Mensagem de retorno)
  }
  </pre>
  </section>
  <section>
    <h3>GET api/produto/listar - Lista todos os produtos</h3>
    
  <h4>Resposta</h4>
  <pre>
  {
    "sucesso": (true/false),
    "data": [{
      "id": "98e82e66-d132-4077-94bd-60dc0f4ffe30",
      "codigo": "PROD-100",
      "nome": "Caneta Vermelha BIC",
      "preco": 130.90,
      "quantidadeEstoque": 10,
      "estoqueMinimoVenda": 10,
      "idCategoria": "9c8a7b6e-1234-4abc-9def-123456789abc",
      "descricao": "Descrição do produto...",
      "disponivel": true
    },{
      "id": "98e82e66-d132-4077-94bd-60dc0f4ffe30",
      "codigo": "PROD-101",
      "nome": "Caneta Verde BIC",
      "preco": 130.90,
      "quantidadeEstoque": 10,
      "estoqueMinimoVenda": 10,
      "idCategoria": "9c8a7b6e-1234-4abc-9def-123456789abc",
      "descricao": "Descrição do produto...",
      "disponivel": true
    }...],
    "message": (Mensagem de retorno)
  }
</pre>
  </section>
  <section>
    <h3>GET api/produto/obtem/{codigo} - Obtém produto pelo código</h3>
    
  <h4>Resposta</h4>
  <pre>
{
    "sucesso": (true/false),
    "data": {
      "id": "98e82e66-d132-4077-94bd-60dc0f4ffe30",
      "codigo": "PROD-100",
      "nome": "Caneta Vermelha BIC",
      "preco": 130.90,
      "quantidadeEstoque": 10,
      "estoqueMinimoVenda": 10,
      "idCategoria": "9c8a7b6e-1234-4abc-9def-123456789abc",
      "descricao": "Descrição do produto...",
      "disponivel": true
    },
    "message": (Mensagem de retorno)
}
</pre>
  </section>
  <section>
    <h3>GET api/produto/buscar/{texto} - Busca produto por texto em código, nome ou descrição</h3>
    
  <h4>Resposta</h4>
  <pre>
{
    "sucesso": (true/false),
    "data": [{
      "id": "98e82e66-d132-4077-94bd-60dc0f4ffe30",
      "codigo": "PROD-100",
      "nome": "Caneta Vermelha BIC",
      "preco": 130.90,
      "quantidadeEstoque": 10,
      "estoqueMinimoVenda": 10,
      "idCategoria": "9c8a7b6e-1234-4abc-9def-123456789abc",
      "descricao": "Descrição do produto...",
      "disponivel": true
    },{
      "id": "98e82e66-d132-4077-94bd-60dc0f4ffe30",
      "codigo": "PROD-101",
      "nome": "Caneta Verde BIC",
      "preco": 130.90,
      "quantidadeEstoque": 10,
      "estoqueMinimoVenda": 10,
      "idCategoria": "9c8a7b6e-1234-4abc-9def-123456789abc",
      "descricao": "Descrição do produto...",
      "disponivel": true
    }...],
    "message": (Mensagem de retorno)
}
  </section>
  
</section>

<section>
  <h2>🔗 Integração entre Serviços</h2>
  <p>
    Os microserviços se comunicam via HTTP (APIs REST).
  </p>

  <p>Exemplo:</p>
  <ul>
    <li>Serviço de pedidos consome:
      <ul>
        <li>Usuários</li>
        <li>Produtos</li>
        <li>Pagamentos</li>
        <li>Estatísticas</li>
      </ul>
    </li>
  </ul>
</section>

<section>
  <h2>🗄️ Banco de Dados</h2>
  <ul>
    <li>PostgreSQL</li>
    <li>Um banco de dados para cada microserviço</li>
    <li>Scripts disponíveis na pasta <code>/database</code></li>
  </ul>
</section>

<section>
  <h2>⚙️ Configuração do Ambiente (Desenvolvimento)</h2>

  <p>
    Para rodar o projeto localmente, é necessário configurar o ambiente de desenvolvimento com o .NET SDK e criar os microserviços.
  </p>

  <h3>🧩 Pré-requisitos</h3>
  <ul>
    <li>.NET SDK 8.0 ou superior</li>
    <li>PostgreSQL</li>
    <li>Git</li>
  </ul>

  <h3>📥 Clonar o repositório</h3>
  <pre>
git clone https://github.com/seu-usuario/seu-repositorio.git
cd marketplace-microservices
  </pre>

  <h3>🧱 Criação dos Microserviços</h3>
  <p>
    Cada microserviço foi criado como um projeto independente utilizando ASP.NET Web API.
  </p>

  <pre>
cd src/nome-do-service
dotnet new webapi
  </pre>

  <p>
    Esse processo deve ser repetido para cada serviço (users, products, cart, orders, etc).
  </p>

  <p>
    A API estará disponível em <code>http://localhost:porta</code>.
  </p>

  <h3>💡 Observações</h3>
  <ul>
    <li>Cada microserviço é executado de forma independente</li>
    <li>Cada serviço possui seu próprio banco de dados</li>
    <li>A aplicação MVC consome os serviços via HTTP</li>
  </ul>
</section>

<section>
  <h2>🚀 Execução</h2>
  <ol>
    <li>Clonar o repositório</li>
    <li>Configurar o PostgreSQL</li>
    <li>Executar os scripts SQL</li>
    <li>Rodar os microserviços</li>
    <li>Rodar o projeto MVC</li>
  </ol>
</section>

<section>
  <h2>📚 Trabalho Acadêmico</h2>
  <p>
    Projeto desenvolvido para aplicação prática dos conceitos de arquitetura de microserviços,
    integração de APIs e desenvolvimento web.
  </p>
</section>
