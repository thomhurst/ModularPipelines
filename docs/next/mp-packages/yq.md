# yq Package

Strongly typed yq YAML-processing commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Yq
```

Required command-line tool: `yq`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Yq`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseYqModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var yq = context.Tools.Yq;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Yq integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
