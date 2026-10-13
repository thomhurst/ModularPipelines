# .NET Package

Strongly typed .NET CLI commands, builders, and TRX test-result parsing.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.DotNet
```

Required command-line tool: `dotnet`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.DotNet`
* `context.Tools.Trx`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.DotNet.Options;



public class UseDotNetModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.DotNet.Workload.ListAsync(

            new DotNetWorkloadListOptions(),

            cancellationToken: cancellationToken);

    }

}
```

The package exposes generated options records for its supported CLI commands.

## SDK diagnostics[​](#sdk-diagnostics "Direct link to SDK diagnostics")

All generated SDK command records inherit `Diagnostics` from `DotNetOptions`. Set it to `true` to emit `--diagnostics` before the complete command path, for example `dotnet --diagnostics tool list --global`. `false` and `null` omit it.

```
new DotNetToolListOptions { Diagnostics = true, Global = true };
```

This is the SDK's diagnostic switch, distinct from command-specific verbosity or diagnostic-file options. Verbosity is not supported by every command and stays on the applicable command records. Runtime-host settings such as `--fx-version`, `--roll-forward`, and `--runtimeconfig`, and root information actions such as `--info` and `--version`, are not inherited SDK execution options. See the [dotnet command reference](https://learn.microsoft.com/dotnet/core/tools/dotnet).
