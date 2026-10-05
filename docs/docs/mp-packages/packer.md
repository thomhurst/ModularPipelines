---
title: Packer Package
---

# Packer Package

Strongly typed Packer machine-image build commands.

## Installation

```shell
dotnet add package ModularPipelines.Packer
```

Required command-line tool: `packer`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Packer`

## Module example

```csharp

public class UsePackerModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var packer = context.Tools.Packer;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Packer integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Global output setting

Every command inherits `MachineReadable` from `PackerOptions`:

```csharp
using ModularPipelines.Packer.Options;

var options = new PackerBuildOptions("image.pkr.hcl")
{
    MachineReadable = true,
    Color = false,
};
// packer -machine-readable build -color=false image.pkr.hcl
```

Packer's [root argument parser](https://github.com/hashicorp/packer/blob/v1.16.1/main.go)
consumes the exact `-machine-readable` token before command dispatch. The generated
flag therefore uses one hyphen and appears before the command. `false` or `null`
omits it; Packer does not accept `-machine-readable=false` as a root setting.
The root help does not list this flag, so its inherited definition comes from
Packer's [documented machine-readable mode](https://developer.hashicorp.com/packer/docs/commands#machine-readable-output).

Color, timestamps, and debug are command-specific settings. They are not promoted
to the base record. Follow Packer's documented restriction against combining
machine-readable output with interactive build debug mode. Help and version are
operations, not inherited settings.

### V4 migration

`MachineReadable` moves from individual command records to their base record;
existing initializers keep the same property name. Reflection code that reads only
declared properties should also inspect inherited properties.

`Color` on build options and `Write` on formatting options remain nullable
booleans, but now render explicit values. `Color = false` emits `-color=false`,
and `Write = false` emits `-write=false`; neither silently omits the option.
Leaving either property `null` preserves Packer's default. Other switches retain
their documented single-hyphen spelling and stay after their command.
