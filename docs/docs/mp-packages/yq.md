---
title: yq Package
---

# yq Package

Strongly typed yq YAML-processing commands.

## Installation

```shell
dotnet add package ModularPipelines.Yq
```

Required command-line tool: `yq`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Yq`

## Module example

```csharp

public class UseYqModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var yq = context.Tools.Yq;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Yq integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Shared settings

`YqOptions` declares the 46 persistent settings shared by `YqEvalOptions` and
`YqEvalAllOptions`. Existing property names remain available through inheritance.
Settings render before the subcommand; expression and file operands render afterward.

```csharp
var options = new YqEvalOptions
{
    InputFormat = "yaml",
    OutputFormat = "json",
    Indent = 2,
    UnwrapScalar = false,
    ExpressionArgument = ".items[]",
    YamlFile1 = ["input.yaml"],
};
```

`Expression` sets the explicit `--expression` option. `ExpressionArgument` remains
the positional expression. Default-true Boolean settings support explicit `false`.
Short aliases are case-sensitive: `-C` selects colors, while `-c` selects compact
sequence indentation. No persistent setting is a repeated collection or credential.
The root-only `--version` action is not inherited by processing commands.

This scope follows [yq 4.54.1's persistent flag registrations](https://github.com/mikefarah/yq/blob/v4.54.1/cmd/root.go).
The command coverage remains `eval` and `eval-all`; utility help and completion
commands do not add generated command groups.
