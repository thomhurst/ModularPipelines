---
title: Eksctl Package
---

# Eksctl Package

`ModularPipelines.Eksctl` provides strongly typed access to the eksctl CLI for
creating and managing Amazon EKS clusters and their resources.

## Installation

```shell
dotnet add package ModularPipelines.Eksctl
```

The `eksctl` executable must be installed and available on `PATH` when the pipeline runs.

## Create a cluster

```csharp
using ModularPipelines.Eksctl.Options;

var result = await context.Tools.Eksctl.Create.ClusterAsync(
    new EksctlCreateClusterOptions
    {
        Name = "production",
        Region = "eu-west-2",
        Zones = ["eu-west-2a", "eu-west-2b"],
        Nodes = 3,
        Managed = true,
    },
    cancellationToken: cancellationToken);
```

This renders the equivalent of:

```shell
eksctl create cluster --name=production --region=eu-west-2 --zones=eu-west-2a --zones=eu-west-2b --nodes=3 --managed=true
```

## Update cluster logging

```csharp
using ModularPipelines.Eksctl.Enums;

var result = await context.Tools.Eksctl.Utils.UpdateClusterLoggingAsync(
    new EksctlUtilsUpdateClusterLoggingOptions
    {
        Cluster = "production",
        Approve = true,
        EnableTypes =
        [
            EksctlUtilsUpdateClusterLoggingEnableTypes.Api,
            EksctlUtilsUpdateClusterLoggingEnableTypes.Audit,
        ],
    },
    cancellationToken: cancellationToken);
```

Eksctl obtains AWS credentials from the standard AWS credential chain. The generated API
does not expose access-key, secret-key, session-token, or password command-line options.

## Inherited settings and V4 migration

All command options inherit `Color`, `Dumplogs`, and `Verbose` from `EksctlOptions`.
These correspond to eksctl's persistent `--color` (`-C`), `--dumpLogs` (`-d`),
and `--verbose` (`-v`) flags. For example:

```csharp
var options = new EksctlGetClusterOptions
{
    Color = "false",
    Dumplogs = true,
    Verbose = 0,
    Region = "eu-west-2",
    Profile = "production",
};
```

The inherited settings render before `get cluster`; Region and Profile remain
command-local and render after it. Color is a string, including the CLI's
`true`, `false`, and `fabulous` values. Verbose accepts zero. Dumplogs emits a
presence flag when true; false and null leave the CLI's false default unchanged.

Existing command initializers keep the same property names and types. Code that
inspects only properties declared directly on each command should now inspect
inherited properties too. Region, Profile, output formatting, and resource-specific
options are not universally inherited and remain on their applicable commands.
