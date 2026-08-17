# Grype Package

Strongly typed Grype vulnerability-scanning commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Grype
```

Required command-line tool: `grype`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Grype`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseGrypeModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var grype = context.Tools.Grype;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Grype integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
