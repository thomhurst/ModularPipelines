# Skopeo Package

Strongly typed Skopeo container-image commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Skopeo
```

Required command-line tool: `skopeo`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Skopeo`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseSkopeoModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var skopeo = context.Tools.Skopeo;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Skopeo integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
