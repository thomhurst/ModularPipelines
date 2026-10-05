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
using ModularPipelines.ArgoCd.Options;

var result = await context.Tools.ArgoCd.App.GetAsync(
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
var result = await context.Tools.ArgoCd.ApplicationSet.CreateAsync(
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

## Shared connection and logging settings

Command records inherit 26 persistent settings from `ArgoCdOptions`, including
`Server`, `AuthToken`, `Config`, `Header`, logging settings, and port forwarding.
They render in the selected command's flag scope, after its command path and operands:

```csharp
using ModularPipelines.Models;

var result = await context.Tools.ArgoCd.App.GetAsync(
    new ArgoCdAppGetOptions("guestbook")
    {
        GrpcWeb = CliOptionValue.Bare,
        HttpRetryMax = 0,
        Insecure = "false",
    },
    cancellationToken: cancellationToken);
```

This renders `argocd app get guestbook --grpc-web --http-retry-max=0 --insecure=false`.
Boolean persistent settings use `CliOptionValue`: a string such as `"false"` supplies
an explicit value, `CliOptionValue.Bare` supplies the bare flag, and `null` omits it.
`Header` accepts repeated values, retains the `-H` alias, and masks each complete
header value in command logs. Certificate/key paths remain ordinary path values.

Command and group settings retain their scope. `AppNamespace` stays on application
commands. Administrative Kubernetes commands retain their own `Server` declaration;
it identifies the Kubernetes API server, not the Argo CD API server. `configure`
retains its command-local `PromptsEnabled` setting. Setting a compatible shadow
through either the base or command record shares storage and emits the flag once.

### Migration

Replace manually supplied persistent arguments with inherited properties and avoid
supplying both. Code using `BindingFlags.DeclaredOnly` must also inspect
`ArgoCdOptions`. Persistent logging/compression enum values now use the shared base
enums; select values through the inherited property's enum type. Replace Boolean
assignments such as `PromptsEnabled = false` with `PromptsEnabled = "false"`.
Existing commands and version APIs are unchanged by this inheritance audit.
