---
title: Hadolint Package
---

# Hadolint Package

`ModularPipelines.Hadolint` provides strongly typed access to the Hadolint Dockerfile linter.

## Installation

```shell
dotnet add package ModularPipelines.Hadolint
```

The `hadolint` executable must be installed and available on `PATH` when the pipeline runs.

## Lint Dockerfiles

```csharp
using ModularPipelines.Hadolint.Enums;
using ModularPipelines.Hadolint.Extensions;
using ModularPipelines.Hadolint.Options;

var result = await context.Hadolint().Execute(
    new HadolintExecuteOptions
    {
        Dockerfiles = ["Dockerfile", "build/Dockerfile"],
        Format = HadolintFormat.Sarif,
        Error = ["DL3006"],
        Ignore = ["DL3008"],
        FailureThreshold = HadolintFailureThreshold.Warning,
    },
    cancellationToken: cancellationToken);
```

This renders the equivalent of:

```shell
hadolint Dockerfile build/Dockerfile --format sarif --error DL3006 --ignore DL3008 --failure-threshold warning
```

Rule-level overrides, ignored rules, trusted registries, and required-label schemas accept
multiple values and emit their option once per value.
