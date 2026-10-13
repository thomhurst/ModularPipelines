# Newman Package

Strongly typed Newman commands for Postman collections.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Newman
```

Required command-line tool: `newman`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Newman`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseNewmanModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var newman = context.Tools.Newman;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Newman integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
