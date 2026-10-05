---
title: Skopeo Package
---

# Skopeo Package

Strongly typed Skopeo container-image commands.

## Installation

```shell
dotnet add package ModularPipelines.Skopeo
```

Required command-line tool: `skopeo`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Skopeo`

## Module example

```csharp

public class UseSkopeoModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var skopeo = context.Tools.Skopeo;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Skopeo integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Shared settings and command scope

All command options inherit nine public persistent settings from `SkopeoOptions`:
`CommandTimeout`, `Debug`, `InsecurePolicy`, `OverrideArch`, `OverrideOs`,
`OverrideVariant`, `Policy`, `RegistriesD`, and `Tmpdir`.
`RegistriesD` preserves the CLI spelling `--registries.d`.
Shared settings render before the subcommand; image operands and local settings
remain after it.

```csharp
var options = new SkopeoInspectOptions("docker://registry.example/image:latest")
{
    CommandTimeout = "30s",
    RegistriesD = "./registries.d",
    Debug = "false",
    TlsVerify = "false",
};
```

This renders as `skopeo --command-timeout=30s --debug=false
--registries.d=./registries.d inspect docker://registry.example/image:latest
--tls-verify=false` (one command line).

Boolean settings that accept an optional value use `CliOptionValue?`.
Use `"true"` or `"false"` for an explicit value, `CliOptionValue.Bare` for
an unvalued switch, and `null` to omit it. When migrating existing TLS settings,
replace C# boolean assignments with these values. `TlsVerify`, `SrcTlsVerify`,
and `DestTlsVerify` remain on commands that support them. The hidden deprecated
root `--tls-verify` flag is not promoted to a shared setting.

`Creds`, `SrcCreds`, and `DestCreds` contain credential pairs and are masked in
logs. Authentication-file paths remain visible. Login's local `Verbose` switch
keeps its `-v` alias; root help and version controls are not shared settings.
