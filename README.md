# ✅ TaskFlow API

## 🎯 Sobre o Projeto
A **TaskFlow API** é uma Web API RESTful desenvolvida com .NET, Entity Framework Core e SQL Server para gerenciamento e acompanhamento do ciclo de vida de tarefas de uma equipe (Pendente → EmAndamento → Concluida).

## 🛠 Tecnologias Utilizadas
- .NET 10 (ASP.NET Core Web API)
- Entity Framework Core 10 + Migrations
- SQL Server
- Swagger / OpenAPI (Swashbuckle)

## 📋 Pré-requisitos
- [.NET SDK 10](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express ou Docker)
- Ferramenta do EF Core: `dotnet tool install --global dotnet-ef`

## 🚀 Como Executar o Projeto
1. Clonar o repositório
   ```bash
   git clone <URL_DO_REPOSITORIO>
   cd TaskFlow/TaskFlow.API
   ```
2. Configurar a Connection String em `TaskFlow.API/appsettings.json`
   - Windows (autenticação integrada):
     `Server=localhost;Database=TaskFlowDb;Trusted_Connection=True;TrustServerCertificate=True;`
   - SQL Server com usuário/senha (ex.: Docker):
     `Server=localhost,1433;Database=TaskFlowDb;User Id=sa;Password=SuaSenha@123;TrustServerCertificate=True;`
3. Criar o banco com as migrations
   ```bash
   dotnet ef database update
   ```
   > Alternativa: executar o script `Database/script.sql` no SQL Server.
4. Executar a aplicação
   ```bash
   dotnet run
   ```
5. Acessar o Swagger: `https://localhost:7100/swagger`

### SQL Server via Docker (opcional)
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SuaSenha@123" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

## 🏗 Arquitetura
```
TaskFlow.API/
 ├── Controllers/     -> Rotas HTTP e Status Codes
 ├── Services/        -> Regras de negócio e validações
 ├── Repositories/    -> Acesso a dados (EF Core)
 ├── Models/
 │   ├── Entities/    -> Tarefa, Categoria
 │   ├── DTOs/        -> Objetos de entrada/saída
 │   └── Enums/       -> StatusTarefa
 ├── Exceptions/      -> NotFoundException, BusinessException
 ├── Middlewares/     -> ExceptionHandlingMiddleware
 └── Data/            -> AppDbContext e Migrations
```
Fluxo: **Controller → Service → Repository → AppDbContext → SQL Server**.

Relacionamento 1:N: uma `Categoria` possui várias `Tarefas`.

## 📌 Endpoints
| Método | Rota | Descrição | Retorno |
|---|---|---|---|
| POST | `/api/tarefas` | Cria tarefa (status Pendente) | 201 |
| GET | `/api/tarefas` | Lista todas | 200 |
| GET | `/api/tarefas?status=Pendente` | Filtra por status (`Pendente`, `EmAndamento`, `Concluida`) | 200 / 400 |
| GET | `/api/tarefas/{id}` | Busca por id | 200 / 404 |
| PUT | `/api/tarefas/{id}` | Atualiza título e descrição | 200 / 400 / 404 |
| DELETE | `/api/tarefas/{id}` | Exclui | 204 / 404 |
| PATCH | `/api/tarefas/{id}/iniciar` | Pendente → EmAndamento | 200 / 400 / 404 |
| PATCH | `/api/tarefas/{id}/concluir` | EmAndamento → Concluida | 200 / 400 / 404 |

### Exemplos
Criar tarefa:
```http
POST /api/tarefas
Content-Type: application/json

{ "titulo": "Implementar login", "descricao": "Tela e endpoint de autenticação", "categoriaId": 1 }
```
Erro padronizado (middleware):
```json
{ "status": 404, "erro": "NotFound", "mensagem": "Tarefa com id 99 não encontrada.", "caminho": "/api/tarefas/99", "dataHora": "2026-10-26T20:00:00" }
```

## 🎥 Vídeo de Apresentação
Inserir link do vídeo.

## 👥 Integrantes
- Seu nome
