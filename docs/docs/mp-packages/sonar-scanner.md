---
title: SonarScanner Package
---

# SonarScanner Package

Strongly typed SonarScanner code-analysis commands.

## Installation

```shell
dotnet add package ModularPipelines.SonarScanner
```

Required command-line tool: `sonar-scanner`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Import `ModularPipelines.SonarScanner.Extensions`, then use this service from a module:

- `context.SonarScanner()`

## Module example

```csharp
using ModularPipelines.SonarScanner.Extensions;

public class UseSonarScannerModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var sonarScanner = context.SonarScanner();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("SonarScanner integration is ready");
    }
}
```

Generated CLI command pages in this section list every available command and its options record.
