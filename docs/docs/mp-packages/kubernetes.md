---
title: Kubernetes Package
---

# Kubernetes Package

Strongly typed kubectl and Kustomize commands.

## Installation

```shell
dotnet add package ModularPipelines.Kubernetes
```

Required command-line tools: `kubectl`, `kustomize`. They must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Kubernetes`
- `context.Tools.Kustomize`

## Module example

```csharp
using ModularPipelines;
using ModularPipelines.Kubernetes.Options;

public class UseKubernetesModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.Kubernetes.Config.ViewAsync(
            new KubernetesConfigViewOptions(),
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Kustomize inherited error diagnostics

Kustomize commands inherit `StackTrace` from `KustomizeOptions`. Setting it to `true` renders `--stack-trace` before the command path; false or null omits the flag. For example:

```csharp
new KustomizeBuildOptions { StackTrace = true, Dir = "overlays/test" }
// kustomize --stack-trace build overlays/test
```

The v5.8.2 audit confirms this single inherited setting using root, build, edit, cfg, fn, and nested edit help. The [official root parser](https://github.com/kubernetes-sigs/kustomize/blob/kustomize/v5.8.2/kustomize/commands/commands.go) registers Go's flags as persistent Cobra flags. Root help is a control action. Build/plugin settings and edit options remain command-local; repeated names across commands do not make them universal settings. `StackTrace` has no short alias, value, collection, or secret metadata.

Existing `StackTrace` initializers remain valid after moving the property to the base record. Reflection consumers should include inherited properties instead of using `DeclaredOnly`. This applies only to Kustomize, not the separate kubectl option hierarchy in this package. Version-subcommand generation remains tracked in #5687.
