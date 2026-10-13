# Syft Package

Strongly typed Syft software-bill-of-materials commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Syft
```

Required command-line tool: `syft`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Syft`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseSyftModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var syft = context.Tools.Syft;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Syft integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
