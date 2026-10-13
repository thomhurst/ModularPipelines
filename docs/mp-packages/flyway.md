# Flyway Package

Strongly typed Flyway database migration commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Flyway
```

Required command-line tool: `flyway`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Flyway`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseFlywayModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var flyway = context.Tools.Flyway;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Flyway integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
