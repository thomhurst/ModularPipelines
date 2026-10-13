# SonarScanner Package

`ModularPipelines.SonarScanner` provides strongly typed access to SonarScanner CLI for SonarQube Server and SonarQube Cloud analysis.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.SonarScanner
```

The `sonar-scanner` executable must be installed and available on `PATH` when the pipeline runs.

## Run analysis[​](#run-analysis "Direct link to Run analysis")

```
using ModularPipelines.SonarScanner.Options;



var result = await context.Tools.SonarScanner.ExecuteAsync(

    new SonarScannerExecuteOptions

    {

        ProjectKey = "example-project",

        Sources = "src",

        Organization = "example-organization",

    },

    cancellationToken: cancellationToken);
```

This renders the equivalent of:

```
sonar-scanner -Dsonar.projectKey=example-project -Dsonar.sources=src -Dsonar.organization=example-organization
```

## Authentication[​](#authentication "Direct link to Authentication")

Prefer the `SONAR_TOKEN` environment variable supplied by your CI secret store. If `Token` is set directly, the generated property is marked as secret so Modular Pipelines masks its value in command logs.
