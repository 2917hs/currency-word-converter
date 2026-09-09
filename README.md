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

- `CurrencyConverter.Core/` — conversion logic (parsing, number-to-words). No web dependencies.
- `Presentation/CurrencyConverter.Api/` — ASP.NET Core Web API.
- `Presentation/currency-converter-client/` — React + TypeScript frontend (Vite).

## Prerequisites

- .NET 10 SDK
- Node.js 20+ and npm

## Run

**Backend** (from repo root):
```bash
dotnet run --project Presentation/CurrencyConverter.Api
```
→ `http://localhost:5295` (Swagger UI at `/swagger`)

**Frontend** (from `Presentation/currency-converter-client`):
```bash
npm install
npm run dev
```
→ `http://localhost:5173`

Both must run at once — the frontend calls the API directly.

## Test & validate

Build:
```bash
dotnet build CurrencyWordConverter.sln
```

Try it via the UI, or call the API directly:
```bash
curl -X POST http://localhost:5295/api/currency/convert \
  -H "Content-Type: application/json" \
  -d '{"amount":"25,1","language":"en"}'
# {"amount":"25,1","language":"en","words":"twenty-five dollars and ten cents"}
```

Invalid input (`abc`, `25,100`, over `999999999`, more than one `,`) returns HTTP 400 with a validation message — try these to confirm error handling works.

## API

- `GET /api/languages` → `[{ code, displayName }]`
- `POST /api/currency/convert` with `{ amount, language }` → `{ amount, language, words }`, or 400 on invalid input.

## Notes

- Conversion is entirely server-side; the frontend only renders the API's response.
- `Money` validates its own bounds in its constructor — an invalid amount can't exist as a `Money` value.
- Not yet done: automated tests, frontend styling, configurable CORS/API URL (currently hardcoded to localhost).

## AI usage disclosure

Built with AI assistance (Claude Code). Prompt history included per submission requirements.
