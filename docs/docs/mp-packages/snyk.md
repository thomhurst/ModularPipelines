---
title: Snyk Package
---

# Snyk Package

`ModularPipelines.Snyk` provides strongly typed access to the Snyk CLI for open-source,
container, infrastructure-as-code, code, SBOM, and AI-BOM security workflows.

## Installation

```shell
dotnet add package ModularPipelines.Snyk
```

The `snyk` executable must be installed and available on `PATH` when the pipeline runs.

## Test a container image

```csharp
using ModularPipelines.Snyk.Enums;
using ModularPipelines.Snyk.Extensions;
using ModularPipelines.Snyk.Options;

var result = await context.Snyk().ContainerTest(
    new SnykContainerTestOptions("alpine:3.20")
    {
        SeverityThreshold = SnykSeverityThreshold.High,
        FailOn = SnykFailOn.Patchable,
    },
    cancellationToken: cancellationToken);
```

This renders the equivalent of:

```shell
snyk container test alpine:3.20 --severity-threshold=high --fail-on=patchable
```

## Authentication

Prefer the `SNYK_TOKEN` environment variable supplied by your CI secret store. The generated
API-token, client-secret, and password properties are marked as secrets so Modular Pipelines
masks their values in command logs.
