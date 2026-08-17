# Flyway Package

Strongly typed Flyway database migration commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Flyway
```

Required command-line tool: `flyway`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Flyway.Extensions`, then use this service from a module:

* `context.Flyway()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Flyway.Extensions;



public class UseFlywayModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var flyway = context.Flyway();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Flyway integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.
