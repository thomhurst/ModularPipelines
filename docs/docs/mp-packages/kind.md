---
title: kind Package
---

# kind Package

Strongly typed kind commands for local Kubernetes clusters.

## Installation

```shell
dotnet add package ModularPipelines.Kind
```

Required command-line tool: `kind`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Kind`

## Module example

```csharp

public class UseKindModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var kind = context.Tools.Kind;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Kind integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Inherited logging options

All commands inherit `Quiet` (`--quiet` / `-q`) and `Verbosity` (`--verbosity` / `-v`) from `KindOptions`. These are kind's root persistent flags, confirmed against the [v0.33.0 root parser](https://github.com/kubernetes-sigs/kind/blob/v0.33.0/pkg/cmd/kind/root.go) and captured root, group, and leaf help. They render before the command path:

```csharp
new KindCreateClusterOptions { Quiet = true, Verbosity = 2, Name = "integration" }
// kind --quiet --verbosity=2 create cluster --name=integration
```

`Quiet` is a presence-only flag: false or null omits it. `Verbosity` is an optional integer value; zero is emitted when explicitly set. Neither option contains a secret or accepts a collection. Repeated command-local settings such as `Name`, `KubeConfig`, and `Wait` stay on their applicable command records. Root help/version actions are not inherited settings.

Existing property initializers remain valid when these two properties move to the base record; reflection consumers should use inherited properties rather than `DeclaredOnly`. Version-subcommand generation is tracked separately in #5687.
