# BoilerplateWPF

> Enterprise-style WPF application template (C#/.NET) with layered architecture, EF Core migrations, unit tests and GitLab CI.

**Overview**

- This solution contains projects: `App.UI` (WPF), `App.Infrastructure` (EF Core, persistence), `App.Application`, `App.Domain`, `App.Common`, and `App.UnitTests`.
- Key files: `App.Infrastructure/Persistence/AppDbContext.cs`, `App.Domain/Entities/UserSetting.cs`, `App.UI/ViewModels/MainViewModel.cs`.

**Prerequisites**

- .NET SDK 10.0
- SQL Server (LocalDB is used by default in `appsettings.json` for development)
- `dotnet-ef` tool for migrations (optional): `dotnet tool install --global dotnet-ef`

**Quick Start (local)**

1. Restore dependencies

```bash
dotnet restore src/BoilerplateWPF.sln
```

2. Build

```bash
dotnet build src/BoilerplateWPF.sln -c Debug
```

3. Run (WPF app)

Open `src/App.UI/App.UI.sln` in Visual Studio and Run, or:

```powershell
dotnet run --project src/App.UI -c Debug
```

**Database migrations (EF Core)**

- A design-time factory is provided at `App.Infrastructure/Persistence/DesignTimeDbContextFactory.cs` so EF tools can create the context.
- Default connection string is in `src/App.UI/appsettings.json` as `ConnectionStrings:DefaultConnection` (LocalDB by default).

Create a migration (uses `App.UI` as startup project so configuration is available):

```powershell
dotnet-ef migrations add YourMigrationName --project src\App.Infrastructure --startup-project src\App.UI
```

Apply migrations:

```powershell
dotnet-ef database update --project src\App.Infrastructure --startup-project src\App.UI
```

If the database already contains schema objects, consider creating a baseline migration with `--ignore-changes` or adjust the migration to be idempotent.

**Unit tests & coverage**

Run tests and collect XPlat coverage:

```powershell
dotnet test src/App.UnitTests/App.UnitTests.csproj --collect:"XPlat Code Coverage" --results-directory TestResults --logger "trx;LogFileName=test_results.trx"
```

Generate Cobertura report (reportgenerator):

```powershell
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator -reports:TestResults/**/coverage.*.xml -targetdir:CoverageReport -reporttypes:Cobertura
```

The GitLab CI pipeline included in the repo already uploads `CoverageReport/Cobertura.xml` and prints a `Coverage: <n>%` line for GitLab to pick up coverage metrics.

**CI (GitLab)**

- Pipeline file: `/.gitlab-ci.yml` — stages: restore, build, test, code_quality, publish.
- Runner tag configured via `default.tags` in the CI YAML. Adjust to match your runner if necessary.

**Troubleshooting**

- If you see `Microsoft.Data.SqlClient is not supported on this platform`, the repo guards SQL Server registration to run only on Windows. Ensure migrations or runtime use a Windows runner for SqlClient, or change provider to SQLite for cross-platform tests.
- If builds fail due to locked DLLs, stop any running `App.UI` instances (Visual Studio or running app) before building tests or running `dotnet test`.
- If EF tools cannot create the DbContext at design time, confirm `DefaultConnection` exists in `appsettings.json` (or set env var `ConnectionStrings__DefaultConnection`) and that the design-time factory is present.

**Notes**

- `MainViewModel` now exposes a `UserSettings` observable property and attempts to load user settings from the `IUnitOfWork` repository; loading is guarded to avoid platform-specific failures.
- Keep EF Core package versions aligned between projects to avoid runtime tool mismatches.

File: [README.md](README.md)
