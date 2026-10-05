---
title: Nerdbank.GitVersioning Package
---

# Nerdbank.GitVersioning Package

`ModularPipelines.NerdbankGitVersioning` provides strongly typed access to the `nbgv` CLI.

## Installation

```shell
dotnet add package ModularPipelines.NerdbankGitVersioning
dotnet tool install --global nbgv
```

The `nbgv` executable must be available on `PATH` when the pipeline runs.

## Option scope

This integration targets the .NET `nbgv` tool. Its root help exposes help/version
actions, with no inherited execution settings. `NbgvOptions` therefore has no
generated global properties. Set `Project`, `Format`, and other settings on the
applicable command record; they render after the command name. For example,
`nbgv get-version --project src/MyProject` is valid, while root
`nbgv --project src/MyProject get-version` is rejected.

`NbgvCloudOptions.Version` is a command-local value for the cloud build number,
distinct from the root `--version` action. No global promotion or API change is
needed for the audited .NET tool version 3.10.94. This audit does not describe the
separate Rust distribution. See the
[official nbgv guide](https://dotnet.github.io/Nerdbank.GitVersioning/docs/nbgv-cli.html).

## Read version information

```csharp
using ModularPipelines.NerdbankGitVersioning.Options;

var result = await context.Tools.Nbgv.GetVersionAsync(
    new NbgvGetVersionOptions
    {
        Project = "src/MyProject",
        Format = "json",
    },
    cancellationToken: cancellationToken);
```

## Set cloud build variables

```csharp
var result = await context.Tools.Nbgv.CloudAsync(
    new NbgvCloudOptions
    {
        CommonVars = true,
        Define = ["Channel=stable"],
    },
    cancellationToken: cancellationToken);
```

See the [generated nbgv CLI reference](./cli/nbgv.md) for every supported command and option.
