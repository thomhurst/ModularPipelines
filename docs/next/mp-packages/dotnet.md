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
