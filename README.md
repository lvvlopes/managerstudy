# ContentHub - Sistema de Gerenciamento de Conteúdo

Um sistema completo para gerenciar categorias, fontes e posts de conteúdo, construído com arquitetura em camadas usando ASP.NET Core.

## 📋 Visão Geral

ContentHub é uma aplicação web que permite:
- Gerenciar categorias de conteúdo
- Gerenciar fontes (perfis) de conteúdo
- Gerenciar posts com informações detalhadas
- Visualizar e filtrar posts por categoria

## 🏗️ Arquitetura

O projeto possui uma arquitetura em camadas com separação clara de responsabilidades:

```
ContentHub.Domain (Camada de Domínio)
├── Entities/
│   ├── Categoria.cs
│   ├── Fonte.cs
│   └── Post.cs
│
ContentHub.Application (Camada de Aplicação)
├── DTOs/
│   ├── CategoriaDtos.cs
│   ├── FonteDtos.cs
│   └── PostDtos.cs
├── Interfaces/
│   ├── ICategoriaRepository.cs
│   ├── IFonteRepository.cs
│   ├── IPostRepository.cs
│   ├── ICategoriaService.cs
│   ├── IFonteService.cs
│   └── IPostService.cs
└── Services/
    ├── CategoriaService.cs
    ├── FonteService.cs
    └── PostService.cs
│
ContentHub.Infrastructure (Camada de Infraestrutura)
├── Data/
│   └── ContentHubDbContext.cs
├── Migrations/
│   └── [Migration files]
└── Repositories/
    ├── CategoriaRepository.cs
    ├── FonteRepository.cs
    └── PostRepository.cs
│
ContentHub.API (API REST)
└── Controllers/
    ├── CategoriasController.cs
    ├── FontesController.cs
    └── PostsController.cs
│
ContentHub.Web (Frontend MVC)
├── Controllers/
├── Views/
├── ViewModels/
└── Services/
    └── ContentHubApiClient.cs
```

## 📦 Dependências e Requisitos

### Pré-requisitos
- **.NET 8.0+** - [Download](https://dotnet.microsoft.com/download/dotnet)
- **SQL Server** (LocalDB) - Incluído no Visual Studio ou [Download](https://www.microsoft.com/sql-server/sql-server-downloads)
- **Visual Studio** ou **VS Code** com extensões C#

### NuGet Packages
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.AspNetCore`
- `Microsoft.AspNetCore.Mvc`

## 🚀 Como Compilar

### 1. Restaurar as dependências
```powershell
dotnet restore
```

### 2. Compilar a solução
```powershell
dotnet build
```

## 🗄️ Banco de Dados

### Localização
- **Connection String**: `Server=(localdb)\mssqllocaldb;Database=ContentHubDb;Trusted_Connection=True;`
- **LocalDB**: Geralmente armazenado em `C:\Users\[usuario]\AppData\Local\Microsoft\Microsoft SQL Server Local DB`

### Aplicar Migrations
```powershell
# No diretório raiz do projeto
dotnet ef database update --project src/ContentHub.Infrastructure

# Ou com a pasta do Infrastructure
cd src/ContentHub.Infrastructure
dotnet ef database update
```

## 🎯 Como Rodar

### Backend (API)

#### Opção 1: Rodar na porta padrão (5000)
```powershell
cd d:\Projetos\Instagram
dotnet run --project src/ContentHub.API/ContentHub.API.csproj --urls http://localhost:5000
```

#### Opção 2: Rodar com configuração padrão
```powershell
cd d:\Projetos\Instagram\src\ContentHub.API
dotnet run
```

#### Opção 3: Rodar em modo Release
```powershell
dotnet run --project src/ContentHub.API/ContentHub.API.csproj -c Release
```

**API estará disponível em:**
- HTTP: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger/index.html`

---

### Frontend (Aplicação Web)

#### Opção 1: Rodar do diretório raiz
```powershell
cd d:\Projetos\Instagram
dotnet run --project src/ContentHub.Web/ContentHub.Web.csproj
```

#### Opção 2: Rodar do diretório do projeto
```powershell
cd d:\Projetos\Instagram\src\ContentHub.Web
dotnet run
```

**Frontend estará disponível em:**
- `http://localhost:5226`

---

### Rodar Backend e Frontend Simultaneamente

**Terminal 1 - Backend:**
```powershell
cd d:\Projetos\Instagram
dotnet run --project src/ContentHub.API/ContentHub.API.csproj --urls http://localhost:5000
```

**Terminal 2 - Frontend:**
```powershell
cd d:\Projetos\Instagram
dotnet run --project src/ContentHub.Web/ContentHub.Web.csproj
```

## 🌐 Acessando a Aplicação

### Frontend (Aplicação Web)
```
http://localhost:5226
```
- Interface amigável para visualizar categorias e posts
- Integração automática com API backend

### Backend (API REST)

#### Swagger UI (Documentação interativa)
```
http://localhost:5000/swagger/index.html
```

#### Endpoints Principais

**Categorias:**
- `GET /api/categorias` - Listar todas as categorias

**Fontes:**
- `GET /api/fontes` - Listar todas as fontes
- `GET /api/fontes/{id}` - Obter detalhes da fonte

**Posts:**
- `GET /api/posts?categoria=&page=1&pagesize=10` - Listar posts
- `GET /api/posts/{id}` - Obter detalhes do post

## ⚙️ Configurações

### Backend (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ContentHubDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### Frontend (appsettings.json)
```json
{
  "ContentHubApi": {
    "BaseUrl": "http://localhost:5000"
  }
}
```

**Alterando a URL da API:**
Se quiser rodar o backend em uma porta diferente, atualize a URL em `src/ContentHub.Web/appsettings.json`:
```json
{
  "ContentHubApi": {
    "BaseUrl": "http://localhost:OUTRA_PORTA"
  }
}
```

## 🔧 Solução de Problemas

### Erro: "Porta já em uso"
```powershell
# Verificar processos usando a porta
Get-NetTCPConnection -LocalPort 5000 | Format-Table

# Matar o processo (exemplo para PID 1234)
Stop-Process -Id 1234 -Force
```

### Erro: "Falha ao conectar com o banco de dados"
```powershell
# Verificar se LocalDB está rodando
sqllocaldb info

# Iniciar LocalDB
sqllocaldb start mssqllocaldb

# Recrear o banco de dados
cd src/ContentHub.Infrastructure
dotnet ef database drop
dotnet ef database update
```

### API indisponível no Frontend
- Verifique se o backend está rodando em `http://localhost:5000`
- Confirme que `ContentHubApi:BaseUrl` em `appsettings.json` do Frontend está correto
- Verifique os logs de rede no navegador (F12 > Network)

## 📝 Estrutura de Dados

### Categorias
- **Id**: Identificador único
- **Nome**: Nome da categoria

### Fontes
- **Id**: Identificador único
- **Nome**: Nome da fonte
- **UrlPerfil**: URL do perfil da fonte
- **CategoriaId**: Referência para categoria

### Posts
- **Id**: Identificador único
- **Url**: URL do post original
- **Descricao**: Descrição do post
- **DataPost**: Data do post
- **DataCadastro**: Data de cadastro no sistema
- **FonteId**: Referência para fonte

## 📚 Referências

- [Documentação ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Swagger/OpenAPI](https://swagger.io/)

---

**Desenvolvido com .NET e ASP.NET Core**
