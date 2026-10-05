---
title: Buildah Package
---

# Buildah Package

Strongly typed Buildah commands for building OCI container images.

## Installation

```shell
dotnet add package ModularPipelines.Buildah
```

Required command-line tool: `buildah`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Buildah`

## Module example

```csharp

public class UseBuildahModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var buildah = context.Tools.Buildah;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Buildah integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Shared storage settings and local overrides

Every command inherits 11 public settings from `BuildahOptions`: `CgroupManager`,
`LogLevel`, `RegistriesConf`, `RegistriesConfDir`, `Root`, `Runroot`,
`ShortNameAliasConf`, `StorageDriver`, `StorageOpt`, `UsernsGidMap`, and
`UsernsUidMap`. Hidden profiling/debug settings and root help/version controls
are not exposed as shared options.

```csharp
var options = new BuildahImagesOptions
{
    Root = "./container-storage",
    Runroot = "./container-state",
    StorageOpt = ["overlay.mount_program=/usr/bin/fuse-overlayfs"],
};
```

Settings render after the command, for example
`buildah images --root=./container-storage --runroot=./container-state
--storage-opt=overlay.mount_program=/usr/bin/fuse-overlayfs` (one command line).
Storage options and UID/GID maps accept collections and repeat their switch for
each value.

`build` and `from` retain their command-local UID/GID mapping definitions. Each
local definition replaces the inherited default in the same CLI scope and
renders once. Assignments through `BuildahOptions` and the command record share
the effective value. When migrating `UsernsUidMap` or `UsernsGidMap` from a string,
wrap the value in a collection, such as `["0:1000:1"]`. Other commands inherit
the default mapping settings directly. Local flags such as image-list `All`
(`-a`) remain local; existing credential annotations are preserved.
