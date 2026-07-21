---
title: Liquibase Package
---

# Liquibase Package

The `ModularPipelines.Liquibase` package provides strongly typed options and services for the Liquibase command-line interface. It covers database updates, rollbacks, changelog generation, snapshots, validation, initialization, and the other commands exposed by Liquibase 5.

Install the package and access Liquibase through the pipeline context:

```csharp
using ModularPipelines.Liquibase.Enums;
using ModularPipelines.Liquibase.Extensions;
using ModularPipelines.Liquibase.Options;
using ModularPipelines.Models;

public class UpdateDatabaseModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Liquibase().Update(
            new LiquibaseUpdateOptions("db/changelog.xml", "jdbc:postgresql://localhost/app")
            {
                Username = "app",
                Password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD"),
                ChangelogProperty = [new KeyValue("environment", "production")],
                ShowSummary = LiquibaseShowSummary.Verbose,
            },
            cancellationToken: cancellationToken);
    }
}
```

Required CLI options are constructor parameters. Constrained values use enums, numeric and boolean options keep their native types, and sensitive values such as passwords are marked for log redaction. Liquibase must be installed and available on `PATH` when the pipeline runs.
