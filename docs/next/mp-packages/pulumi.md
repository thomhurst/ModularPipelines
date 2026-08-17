# Pulumi Package

Strongly typed Pulumi infrastructure commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Pulumi
```

Required command-line tool: `pulumi`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Pulumi`

## Module example[​](#module-example "Direct link to Module example")

```


public class UsePulumiModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var pulumi = context.Tools.Pulumi;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Pulumi integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
