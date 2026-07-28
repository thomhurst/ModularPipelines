---
title: Nerdbank.GitVersioning Package
---

# Nerdbank.GitVersioning Package

`ModularPipelines.NerdbankGitVersioning` provides strongly typed access to the `nbgv` CLI.

## Installation

```shell
dotnet add package ModularPipelines.NerdbankGitVersioning
dotnet tool install --global nbgv
```

The `nbgv` executable must be available on `PATH` when the pipeline runs.

## Read version information

```csharp
using ModularPipelines.NerdbankGitVersioning.Extensions;
using ModularPipelines.NerdbankGitVersioning.Options;

var result = await context.Nbgv().GetVersion(
    new NbgvGetVersionOptions
    {
        Project = "src/MyProject",
        Format = "json",
    },
    cancellationToken: cancellationToken);
```

## Set cloud build variables

```csharp
var result = await context.Nbgv().Cloud(
    new NbgvCloudOptions
    {
        CommonVars = true,
        Define = ["Channel=stable"],
    },
    cancellationToken: cancellationToken);
```

See the [generated nbgv CLI reference](./cli/nbgv.md) for every supported command and option.
