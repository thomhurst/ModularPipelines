# kind Package

Strongly typed kind commands for local Kubernetes clusters.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Kind
```

Required command-line tool: `kind`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Kind`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseKindModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var kind = context.Tools.Kind;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Kind integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
