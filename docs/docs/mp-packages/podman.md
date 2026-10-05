---
title: Podman Package
---

# Podman Package

Strongly typed Podman container-management commands.

## Installation

```shell
dotnet add package ModularPipelines.Podman
```

Required command-line tool: `podman`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Podman`

## Module example

```csharp

public class UsePodmanModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var podman = context.Tools.Podman;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Podman integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.


## Root execution settings

Every command options record inherits Podman's public root settings from
`PodmanOptions`. Set `Connection` and `Remote` for a remote service, or `Root`,
`Runroot`, `Runtime`, and `StorageOpt` for local storage and runtime selection.
These settings render before the subcommand, including nested commands.

```csharp
var options = new PodmanPsOptions
{
    Connection = "builder",
    Remote = CliOptionValue.Bare,
    Syslog = "false",
};
// podman --connection=builder --remote --syslog=false ps
```

The optional boolean properties accept `CliOptionValue.Bare`, `"true"`, `"false"`,
or `null` to omit the switch. `CdiSpecDir`, `HooksDir`, `Module`, `RuntimeFlag`, and
`StorageOpt` accept collections and repeat the switch for each value. Short aliases
`-c` and `-r` remain available in option metadata. Identity and TLS key settings
refer to file paths; they are not credential contents.

Compose provider options such as `ProjectName`, `File`, and `Profile` stay on
`PodmanComposeOptions`. A command-local `--identity` or TLS setting configures that
command's target and remains independent of the root connection settings.

### V4 migration

Root execution settings are now available through every options record. Where a
command already had an option with the same name, its generated property receives
a command-specific name. In particular, use inherited `Identity` for the active
connection and the separate local identity property on
`PodmanSystemConnectionAddOptions` for the connection being added. Do not move
Compose provider settings onto `PodmanOptions`.

This surface was audited against Podman 6.1.3 and Docker Compose 5.6.0 help.
