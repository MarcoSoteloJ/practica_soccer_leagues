# Soccer Leagues

A full-stack app to manage soccer leagues and teams, with a many-to-many relationship between them. The backend is built with C# and .NET, and the frontend with Angular and TypeScript.

## Tech stack

- **Backend:** C#, .NET 10, ASP.NET Core and Entity Framework Core
- **Frontend:** Angular 22, TypeScript and Tailwind CSS
- **Database:** SQL Server 2022

## Project decisions

- **Architecture:** the API follows an MVC-style pattern with models and controllers. Angular handles the views.
- **UI:** the layout uses flexbox with Tailwind CSS to arrange the components.
-- **Docker:** an optional Docker Compose file runs SQL Server, for anyone without a local installation or a Windows computer.
- **AI use:** I used Claude Code as support to build this practice and understand the syntax.

## Project structure

- `database/`: SQL script and Docker Compose file
- `backend/api/`: REST API
- `frontend/`: Angular app

## Getting started

You need the .NET 10 SDK, Node.js and a SQL Server instance. You can use a local installation or Docker.

### 1. Set up the database

Use one of these two options to get SQL Server running.

**Option A: local SQL Server**

Use your existing SQL Server installation and go to the next step.

**Option B: Docker**

```bash
cd database
cp .env.example .env
```

Set your password in `.env`. SQL Server requires at least 8 characters, using 3 of these 4 types: uppercase, lowercase, numbers and symbols.

```bash
docker compose up -d
```

To keep your data, stop the container with `docker compose stop`. Running `docker compose down` deletes the database.

**Create the database**

With either option, run `database/create_soccer_db.sql` in your SQL client.
### 2. Start the API

```bash
cd backend/api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=Soccer;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
dotnet run --launch-profile http
```

The API runs at `http://localhost:5137`.

### 3. Start the frontend

```bash
cd frontend
npm install
npx ng serve
```

Open `http://localhost:4200`.


## Possible improvements

- Validate names and player counts in the API too. Angular and SQL enforce them today, so invalid data sent directly to the API returns a `500` error.
