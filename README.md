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
    Desenvolver uma aplicação web baseada em arquitetura de microserviços, aplicando conceitos de APIs REST,
    integração entre serviços, persistência de dados e deploy em cloud.
  </p>
</section>

<section>
  <h2>🧱 Arquitetura</h2>
  <p>
    O sistema é composto por um frontend em ASP.NET MVC que consome múltiplos microserviços independentes.
  </p>

  <h3>Microserviços:</h3>
  <ul>
    <li>Usuário</li>
    <li>Produto</li>
    <li>Categoria</li>
    <li>Carrinho</li>
    <li>Pedido</li>
    <li>Pagamento</li>
    <li>Estatísticas</li>
    <li>Avaliação (Review)</li>
    <li>Busca (Procura)</li>
  </ul>

  <p>
    Cada microserviço possui sua própria API REST e banco de dados independente (PostgreSQL).
  </p>
</section>

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
  <h2>📁 Estrutura do Projeto</h2>

  <pre>
src/        # Microserviços
web/        # Aplicação MVC
docs/       # Documentação
database/   # Scripts SQL
README.md
  </pre>
</section>

<section>
  <h2>🔧 Estrutura dos Microserviços</h2>

  <pre>
Controllers/
Services/
Repositories/
DTO/
Models/
Data/
  </pre>
</section>

<section>
  <h2>🔌 Padrão de Endpoints</h2>

  <pre>
GET    /api/produtos
GET    /api/produtos/{id}
POST   /api/produtos
PUT    /api/produtos/{id}
DELETE /api/produtos/{id}
  </pre>
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
    <li>Um banco por microserviço</li>
    <li>Scripts disponíveis na pasta <code>/database</code></li>
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
