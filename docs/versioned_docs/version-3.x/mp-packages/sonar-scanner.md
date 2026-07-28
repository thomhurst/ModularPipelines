---
title: SonarScanner Package
---

# SonarScanner Package

`ModularPipelines.SonarScanner` provides strongly typed access to SonarScanner CLI for
SonarQube Server and SonarQube Cloud analysis.

## Installation

```shell
dotnet add package ModularPipelines.SonarScanner
```

The `sonar-scanner` executable must be installed and available on `PATH` when the pipeline runs.

## Run analysis

```csharp
using ModularPipelines.SonarScanner.Extensions;
using ModularPipelines.SonarScanner.Options;

var result = await context.SonarScanner().Execute(
    new SonarScannerExecuteOptions
    {
        ProjectKey = "example-project",
        Sources = "src",
        Organization = "example-organization",
    },
    cancellationToken: cancellationToken);
```

This renders the equivalent of:

```shell
sonar-scanner -Dsonar.projectKey=example-project -Dsonar.sources=src -Dsonar.organization=example-organization
```

## Authentication

Prefer the `SONAR_TOKEN` environment variable supplied by your CI secret store. If `Token` is
set directly, the generated property is marked as secret so Modular Pipelines masks its value
in command logs.
