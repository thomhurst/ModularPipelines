# Grype Package

Strongly typed Grype vulnerability-scanning commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Grype
```

Required command-line tool: `grype`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Grype.Extensions`, then use this service from a module:

* `context.Grype()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Grype.Extensions;



public class UseGrypeModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var grype = context.Grype();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Grype integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.
