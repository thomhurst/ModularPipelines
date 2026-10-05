---
title: Flux Package
---

# Flux Package

Strongly typed Flux CLI commands for GitOps workflows.

## Installation

```shell
dotnet add package ModularPipelines.Flux
```

Required command-line tool: `flux`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Flux`

## Module example

```csharp

public class UseFluxModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var flux = context.Tools.Flux;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Flux integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Persistent settings

`FluxOptions` exposes the 23 persistent settings verified against Flux 2.9.6:
Kubernetes identity and impersonation, kubeconfig/context/namespace, API server and TLS,
client rate limits, timeout, and verbosity. They are inherited by generated command records.
Settings continue to render after the command path, preserving Flux's existing argument ordering.

```csharp
using ModularPipelines.Flux.Options;

var options = new FluxGetSourcesGitOptions
{
    Context = "production",
    Namespace = "flux-system",
    AsGroup = ["developers", "readers"],
    AsUserExtra = ["team=platform", "team=delivery"],
    KubeApiQps = 12.5,
    Timeout = "2m",
    AllNamespaces = true,
};
```

`AsGroup` and `AsUserExtra` emit a separate switch for each value. `Namespace` also
supports Flux's `-n` alias. Nullable presence flags omit `null` and `false`, while explicit
numeric zero values are preserved. `Token` is marked as a secret.

The `get` group's selectors, watch/header controls, and all-namespaces switch remain local
rather than becoming root settings. Bootstrap's branch, repository credentials, and installation
settings likewise remain on the relevant command records.

### Receiver tokens

`FluxCreateSecretReceiverOptions.Token` and `FluxTriggerReceiverOptions.Token` represent the
command's webhook token, which replaces Flux's root API bearer-token flag for those commands.
Their command-specific descriptions and secret masking are retained. Setting `Token` through
a `FluxOptions` reference accesses the same value and emits the local switch once. Use kubeconfig
credentials when these receiver commands also need Kubernetes authentication.

## Migration

Existing initializer names remain available. Persistent properties now live on `FluxOptions`,
so reflection code must include inherited properties instead of using `DeclaredOnly`.
The two receiver token replacements remain declared on their command records. No root help/version
controls or group-only settings are promoted. Existing utility-command exclusions are unchanged.

See the [generated Flux reference](./cli/flux.md) for the complete persistent-setting list and
command-local options. Scope evidence and captured help live in the generator's
[Flux fixtures](https://github.com/thomhurst/ModularPipelines/tree/main/tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator.Tests/Fixtures/Flux/2.9.6).
