# DummyJSON API Test Automation

BDD API tests in C# for the public [DummyJSON](https://dummyjson.com/docs) REST API, covering the nine exercises of the coding challenge across Auth, Users, Products and Carts.

## Tech stack

C# / .NET 10, [Reqnroll](https://reqnroll.net) 3 (Gherkin), NUnit 4, [Refit](https://github.com/reactiveui/refit) 8 (HTTP client), System.Text.Json.

## Running the tests

Prerequisite: the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet test
```

Run a single area by tag (`auth`, `users`, `products`, `carts`):

```bash
dotnet test --filter "Category=carts"
```

Settings (base URL, timeout, login credentials) live in `src/DummyJson.Tests/appsettings.json`. The credentials are the public demo user from the DummyJSON docs.

## Project structure

```
src/DummyJson.Tests
├── Features/           Gherkin feature files, numbered in exercise order
├── StepDefinitions/    Step bindings, one class per feature + common steps
├── Tables/             Typed classes for Gherkin data tables
├── Transformations/    StepArgumentTransformations: data table -> *Table class
├── Api/
│   ├── IDummyJsonApi.cs  Refit interface, one method per endpoint
│   ├── Model/          Nested objects shared by requests and responses
│   ├── Request/        Request bodies
│   └── Response/       Response bodies
├── Support/            Scenario context, hooks, settings
└── appsettings.json
```

## Scenarios and endpoints

| # | Exercise | Endpoint | Key validations |
|---|---|---|---|
| 1.1 (1a) | Authenticate and store token | `POST /auth/login` | 200, access and refresh tokens present, access token is a JWT, token stored in scenario context |
| 1.2 (1b) | Current authenticated user | `GET /auth/me` (Bearer) | 200, id = 1, first name = Emily, last name = Johnson |
| 2.1 (2a) | Users with red hair | `GET /users/filter?key=hair.color&value=Red&limit=0` | 200, 25 users returned, `total` = 25, every user's hair colour is Red |
| 2.2 (2b) | Add a user | `POST /users/add` | 201, new id generated, name/age/gender/hair/full address (incl. coordinates) echoed back exactly |
| 3.1 (3a) | Price of iPhone 13 Pro | `GET /products/search?q=iPhone 13 Pro` | 200, exactly one match, price = 1099.99 |
| 3.2 (3b) | 15th product via pagination | `GET /products?limit=1&skip=14&select=id,title,price` | 200, exactly one product, id 15 / title / price 799.99, object contains **only** `id`, `title`, `price` |
| 3.3 (3c) | Update product 121's title | `PUT /products/121` | 200, id = 121, title is the new value |
| 4.1 (4a) | Add 5 products to a cart | `POST /carts/add` | 201, 5 lines / quantity 8, lines match the request, **total discount percentage = 62.11**, line and cart totals add up |
| 4.2 (4b) | Delete a cart | `DELETE /carts/1` | 200, id = 1, `isDeleted = true`, `deletedOn` is about now |

## Development decisions

- **Reqnroll for BDD.** SpecFlow reached end of life in 2024. Reqnroll is its maintained successor, with the same Gherkin and binding model.
- **Refit for HTTP calls.** Each endpoint is declared once as an attribute-decorated method in `IDummyJsonApi`, and Refit handles URLs, query strings, JSON and the bearer header. RestAssured.Net was considered, but it builds every request in code in each step, so Refit needs less code and gives one place to change an endpoint.
- **Independent scenarios, run in parallel.** `ApiContext` is created fresh for every scenario and shared between step classes through context injection. Scenarios that need a token log in themselves, so they can run in any order. This allows NUnit parallel execution at feature and scenario level (`[assembly: Parallelizable(ParallelScope.Fixtures | ParallelScope.Children)]` in `AssemblyInfo.cs`). The shared `HttpClient` and Refit client are thread-safe.
- **Typed data tables.** Gherkin tables are converted into `*Table` classes by `StepArgumentTransformation`s, so step methods take typed parameters and the mapping lives in one place.
- **Testing a mock API.** DummyJSON does not persist writes, so POST, PUT and DELETE tests validate the response (status, echoed values, generated id, `isDeleted`) rather than reading the record back.

## Possible improvements

- Validate responses against a JSON schema.
- Log every request and response (for example a `DelegatingHandler` in the `HttpClient` pipeline) and print the last response body when a scenario fails, to make failures easier to diagnose.
- Add negative scenarios (invalid credentials → 400, expired or missing token → 401, unknown id → 404).
- Add HTML living documentation (Reqnroll LivingDoc) and a GitHub Actions workflow that runs the suite on every push.
- Add retries for transient network errors on the public endpoint (for example a Polly handler in the `HttpClient` pipeline).
