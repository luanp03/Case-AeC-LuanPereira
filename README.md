## Case - Gerenciamento de Endereços (AeC)

Este projeto é uma aplicação Web desenvolvida em ASP.NET Core MVC para o gerenciamento de endereços vinculados a usuários autenticados. O sistema utiliza a API do ViaCEP para preenchimento automático de dados e permite a exportação de relatórios em CSV.

## 🚀 Funcionalidades
* **Autenticação Completa:** Cadastro de novos usuários e sistema de Login com proteção de rotas.

* **Gerenciamento de Endereços (CRUD):** Criar, visualizar, editar e excluir endereços.

* **Segurança de Dados:** Cada usuário visualiza e gerencia apenas os seus próprios endereços.

* **Integração com API Externa:** Consumo da API ViaCEP para busca automática de logradouro, bairro, cidade e UF através do CEP.

* **Exportação de Dados:** Geração de arquivo CSV com a lista de endereços cadastrados do usuário logado.

## 🛠️ Tecnologias Utilizadas
* **Linguagem:** C#

* **Framework:** .NET 8 (ASP.NET Core MVC)

* **ORM:** Entity Framework Core

* **Banco de Dados:** SQL Server

* **Front-end:** Razor Pages, Bootstrap 5 e jQuery (para máscaras e chamadas AJAX)

* **API:** ViaCEP API

## 📦 Como executar o projeto
### Clonar o repositório:

    git clone https://github.com/luanp03/Case-AeC-LuanPereira

### Configurar o Banco de Dados:

    No arquivo appsettings.json, altere a ConnectionString para apontar para o seu servidor local.

### Executar as Migrations:

No Console do Gerenciador de Pacotes, execute:

    Update-Database

Iniciar a aplicação:

    dotnet run

## 📝 Detalhes Técnicos

*   **Arquitetura:** O projeto segue o padrão MVC (Model-View-Controller) garantindo a separação de responsabilidades.
*   **Filtro de Autorização:** Foi utilizado o atributo `[Authorize]` para garantir que apenas usuários autenticados acessem a área de endereços.
*   **Exportação CSV:** Implementação customizada para geração de arquivos CSV, garantindo uma exportação leve e rápida diretamente através do fluxo de dados (Stream) do servidor.