# Pulumi Package

Strongly typed Pulumi infrastructure commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Pulumi
```

Required command-line tool: `pulumi`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Pulumi.Extensions`, then use this service from a module:

* `context.Pulumi()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Pulumi.Extensions;



public class UsePulumiModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var pulumi = context.Pulumi();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Pulumi integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.
