# Currency Word Converter

Converts a dollar amount into words (e.g. `25,1` → "twenty-five dollars and ten cents"), in English or German.

## Assignment

Convert a dollar amount into words.
- Max `999 999 999` dollars, max `99` cents, `,` separates dollars and cents.
- At least two languages, user-selectable, conversion done server-side.

| Input | Output |
|---|---|
| `0` | zero dollars |
| `25,1` | twenty-five dollars and ten cents |
| `999 999 999,99` | nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents |

## Project structure

- `CurrencyConverter.Core/` — conversion logic. No web dependencies.
- `CurrencyConverter.Core.Tests/`, `CurrencyConverter.Api.Tests/` — xUnit tests.
- `Presentation/CurrencyConverter.Api/` — ASP.NET Core Web API.
- `Presentation/currency-converter-client/` — React + TypeScript frontend (Vite).

## Prerequisites

- .NET 10 SDK
- Node.js 20+ and npm

## Run

**Backend** (repo root): `dotnet run --project Presentation/CurrencyConverter.Api`
→ `http://localhost:5295` (Swagger at `/swagger`, health at `/health`)

**Frontend** (`Presentation/currency-converter-client`): `npm install && npm run dev`
→ `http://localhost:5173`

Run both at once — the frontend calls the API directly. CORS origin (backend `appsettings.Development.json`) and API URL (frontend `.env.development`) must match if you change ports.

**Docker** (repo root): `docker compose up --build` → frontend on `:8081`, API on `:8080`.

## Test & validate

```bash
dotnet test CurrencyWordConverter.sln
cd Presentation/currency-converter-client && npm run lint
```

Or call the API directly:
```bash
curl -X POST http://localhost:5295/api/v1/currency/convert \
  -H "Content-Type: application/json" -d '{"amount":"25,1","language":"en"}'
# {"amount":"25,1","language":"en","words":"twenty-five dollars and ten cents"}
```

Invalid input (`abc`, `25,100`, over `999999999`, more than one `,`) returns HTTP 400 with a validation message.

## API

- `GET /api/v1/languages` → `[{ code, displayName }]`
- `POST /api/v1/currency/convert` with `{ amount, language }` → `{ amount, language, words }`, or 400 on invalid input
- `GET /health` → 200 when up

Frontend types (`src/api-types.ts`) are generated from the API, not hand-written: `npm run generate:api-types` (backend must be running).

## Notes

- Conversion is entirely server-side; the frontend only renders the API's response.
- Errors go through one global exception handler, not per-endpoint try/catch.
- Convert endpoint is rate-limited (30 req/min); Swagger is dev-only; HSTS on outside Development.
- Dockerfiles are untested in this environment (no Docker available here) — verify before relying on them.
- Not yet done: frontend styling.

## AI usage disclosure

Built with AI assistance (Claude Code). Prompt history included per submission requirements.
