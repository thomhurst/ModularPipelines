---
title: Pulumi Package
---

# Pulumi Package

Strongly typed Pulumi infrastructure commands.

## Installation

```shell
dotnet add package ModularPipelines.Pulumi
```

Required command-line tool: `pulumi`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Pulumi`

## Module example

```csharp

public class UsePulumiModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var pulumi = context.Tools.Pulumi;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Pulumi integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Inherited settings and V4 migration

`PulumiOptions` now declares the thirteen public persistent settings from Pulumi
3.267.0: `Color`, `Cwd`, `DisableIntegrityChecking`, `Emoji`,
`FullyQualifyStackNames`, `Logflow`, `Logtostderr`, `Memprofilerate`,
`NonInteractive`, `OtelTraces`, `Profiling`, `Tracing`, and `Verbose`.
Generated command records inherit these properties, which render before the
subcommand path. Existing initializers retain their property names and types;
reflection over command records must include inherited properties.

```csharp
using ModularPipelines.Pulumi.Options;

var options = new PulumiStackListOptions
{
    Cwd = "infra",
    Emoji = false,
    NonInteractive = true,
    Verbose = 0,
    Output = "json",
};
```

This renders `pulumi --cwd=infra --emoji=false --non-interactive --verbose=0 stack list --output=json`.
`Emoji = false` now emits an explicit value, allowing callers to disable Pulumi's
macOS default. `Emoji = null` leaves the tool default unchanged. Other boolean
globals keep their presence-flag behavior: true emits the flag; false/null omit it.
Zero integer values remain explicit, and each scalar string keeps its token boundary.

Stack/project selectors and update arguments remain command-local. `Env` belongs
only to the environment command group and renders after that command path.
`PulumiEnvRunOptions` keeps its command and arguments after `--`, so forwarded
arguments cannot become Pulumi global settings. Help, root version controls, and
hidden flags are not added to the public global surface.

The audit uses [version-pinned parser registration](https://github.com/pulumi/pulumi/blob/v3.267.0/pkg/cmd/pulumi/pulumi.go#L451-L477)
and captured help. The [generated command reference](cli/pulumi.md) lists the
current package surface.
