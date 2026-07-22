---
title: jq Package
---

# jq Package

`ModularPipelines.Jq` provides strongly typed access to the jq JSON processor CLI.

## Installation

```shell
dotnet add package ModularPipelines.Jq
```

The `jq` executable must be installed and available on `PATH` when the pipeline runs.

## Process JSON

```csharp
using ModularPipelines.Jq.Extensions;
using ModularPipelines.Jq.Options;
using ModularPipelines.Models;

var result = await context.Jq().Execute(
    new JqExecuteOptions
    {
        RawOutput = true,
        Indent = 2,
        Arg = [new CliOptionValuePair("environment", "production")],
        Filter = ".deployments[] | select(.environment == $environment)",
        InputFiles = ["deployments.json"],
    },
    cancellationToken: cancellationToken);
```

This renders:

```shell
jq --raw-output --indent 2 --arg environment production \
  '.deployments[] | select(.environment == $environment)' deployments.json
```

Filter and input values are emitted after options. jq options taking a name/value pair—such as
`--arg`, `--argjson`, `--slurpfile`, and `--rawfile`—use `CliOptionValuePair` so both values become
separate command-line arguments.
