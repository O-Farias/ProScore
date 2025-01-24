# ProScore API

Uma API RESTful para gerenciar eventos de futebol, permitindo o cadastro de times, jogadores, partidas e eventos como gols e cartões. Ideal para organizar competições ou acompanhar dados de jogos.

## 🚀 Tecnologias

- .NET 8.0 (https://dotnet.microsoft.com/download/dotnet/8.0)
- Entity Framework Core 9.0 (https://docs.microsoft.com/ef/core/)
- SQL Server (https://www.microsoft.com/sql-server)
- xUnit para testes (https://xunit.net/)
- Moq para mocking em testes (https://github.com/moq/moq4)
- FluentAssertions para asserções em testes (https://fluentassertions.com/)

## 📋 Pré-requisitos

- .NET SDK 8.0
- SQL Server
- Uma IDE (Visual Studio Code, Visual Studio, etc.)

## 🔧 Instalação

1. Clone o repositório:
   git clone https://github.com/O-Farias/ProScore.git

2. Configure a string de conexão no arquivo appsettings.json:
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=ProScoreDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }

3. Execute as migrations:
   dotnet ef database update

4. Execute o projeto:
   dotnet run --project src/ProScore.Api



## 📌 Endpoints da API

Times:
- GET /api/team - Lista todos os times
- GET /api/team/{id} - Obtém um time específico
- POST /api/team - Cria um novo time
- PUT /api/team/{id} - Atualiza um time
- DELETE /api/team/{id} - Remove um time

Jogadores:
- GET /api/player - Lista todos os jogadores
- GET /api/player/{id} - Obtém um jogador específico
- POST /api/player - Cria um novo jogador
- PUT /api/player/{id} - Atualiza um jogador
- DELETE /api/player/{id} - Remove um jogador

Partidas:
- GET /api/match - Lista todas as partidas
- GET /api/match/{id} - Obtém uma partida específica
- POST /api/match - Cria uma nova partida
- PUT /api/match/{id} - Atualiza uma partida
- DELETE /api/match/{id} - Remove uma partida

Eventos:
- GET /api/event/match/{matchId} - Lista eventos de uma partida
- POST /api/event - Cria um novo evento
- PUT /api/event/{id} - Atualiza um evento
- DELETE /api/event/{id} - Remove um evento

## ⚙️ Executando os Testes

Execute o comando:
dotnet test src/ProScore.Tests

## ✨ Funcionalidades

- Gerenciamento completo de times e jogadores
- Registro e acompanhamento de partidas
- Registro de eventos durante as partidas (gols, cartões, etc.)
- API RESTful com documentação Swagger
- Testes automatizados para controllers e services


## 📝 Licença

Este projeto está sob a licença MIT - veja o arquivo LICENSE para detalhes.