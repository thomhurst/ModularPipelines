# Flux Package

Strongly typed Flux CLI commands for GitOps workflows.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Flux
```

Required command-line tool: `flux`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Flux`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseFluxModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var flux = context.Tools.Flux;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Flux integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
