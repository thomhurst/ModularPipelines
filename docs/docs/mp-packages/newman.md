---
title: Newman Package
---

# Newman Package

Strongly typed Newman commands for Postman collections.

## Installation

```shell
dotnet add package ModularPipelines.Newman
```

Required command-line tool: `newman`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Newman`

## Module example

```csharp
using ModularPipelines;
using ModularPipelines.Models;
using ModularPipelines.Newman.Options;

public class UseNewmanModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.Newman.RunAsync(
            new NewmanRunOptions("collection.json")
            {
                Reporters = "cli,json",
                TimeoutRequest = "3000",
                Bail = CliOptionValue.Bare,
            },
            cancellationToken: cancellationToken);
    }
}
```

The package exposes generated options records for its supported CLI commands.

## Optional values and migration

The generated API reflects Newman 6.2.2. `Reporters`, `Bail`, `DelayRequest`,
`Timeout`, `TimeoutRequest`, and `TimeoutScript` use `CliOptionValue?`, matching
Newman's optional-value syntax. Set a string to pass a value, `CliOptionValue.Bare`
to emit only the switch, or `null` to omit it. Replace former `true` initializers
with `CliOptionValue.Bare` and former `false` initializers with `null`.

Reporter names and bail modifiers use a single comma-separated string. Numeric
optional values are strings in milliseconds, for example `Timeout = "10000"`.
`IterationCount` is an `int?`; replace a quoted number with an integer initializer.
`Folder`, `GlobalVar`, and `EnvVar` remain collections that repeat their switches.
`SuppressExitCode` is now available as a boolean flag, including its `-x` alias.

The collection operand renders before run options, so bare optional switches cannot
consume its filename as their value. Run options remain specific to `newman run`;
root help/version actions are not inherited globals. `PostmanApiKey` and
`SslClientPassphrase` are masked in command logs.
