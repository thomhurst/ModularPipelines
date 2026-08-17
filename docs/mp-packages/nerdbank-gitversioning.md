# Nerdbank.GitVersioning Package

`ModularPipelines.NerdbankGitVersioning` provides strongly typed access to the `nbgv` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.NerdbankGitVersioning

dotnet tool install --global nbgv
```

The `nbgv` executable must be available on `PATH` when the pipeline runs.

## Read version information[​](#read-version-information "Direct link to Read version information")

```
using ModularPipelines.NerdbankGitVersioning.Extensions;

using ModularPipelines.NerdbankGitVersioning.Options;



var result = await context.Nbgv().GetVersion(

    new NbgvGetVersionOptions

    {

        Project = "src/MyProject",

        Format = "json",

    },

    cancellationToken: cancellationToken);
```

## Set cloud build variables[​](#set-cloud-build-variables "Direct link to Set cloud build variables")

```
var result = await context.Nbgv().Cloud(

    new NbgvCloudOptions

    {

        CommonVars = true,

        Define = ["Channel=stable"],

    },

    cancellationToken: cancellationToken);
```

See the [generated nbgv CLI reference](/ModularPipelines/docs/mp-packages/cli/nbgv.md) for every supported command and option.
