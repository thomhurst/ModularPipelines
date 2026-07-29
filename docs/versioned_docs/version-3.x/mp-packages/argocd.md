---
title: Argo CD Package
---

# Argo CD Package

`ModularPipelines.ArgoCd` provides strongly typed access to the Argo CD CLI, including
applications, ApplicationSets, clusters, projects, repositories, certificates, accounts,
and administrative commands.

## Installation

```shell
dotnet add package ModularPipelines.ArgoCd
```

The `argocd` executable must be installed and available on `PATH` when the pipeline runs.

## Get an application

```csharp
using ModularPipelines.ArgoCd.Enums;
using ModularPipelines.ArgoCd.Extensions;
using ModularPipelines.ArgoCd.Options;

var result = await context.ArgoCd().App.Get(
    new ArgoCdAppGetOptions("guestbook")
    {
        AppNamespace = "argocd",
        Output = ArgoCdAppGetOutput.Json,
        Refresh = true,
    },
    cancellationToken: cancellationToken);
```

This renders the equivalent of:

```shell
argocd app get guestbook --app-namespace=argocd --output=json --refresh
```

## Create ApplicationSets

```csharp
var result = await context.ArgoCd().ApplicationSet.Create(
    new ArgoCdApplicationSetCreateOptions(["apps.yaml", "more-apps.yaml"])
    {
        DryRun = true,
        Output = ArgoCdApplicationSetCreateOutput.Yaml,
    },
    cancellationToken: cancellationToken);
```

## Authentication

Set `AuthToken` on an options record or provide `ARGOCD_AUTH_TOKEN` to the process. Generated
token, password, private-key, and credential properties are marked as secrets so Modular
Pipelines masks their values in command logs.
