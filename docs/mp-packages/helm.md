# Helm Package

Strongly typed Helm package-management commands for Kubernetes.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Helm
```

Required command-line tool: `helm`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Helm.Extensions`, then use this service from a module:

* `context.Helm()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Helm.Extensions;



public class UseHelmModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var helm = context.Helm();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Helm integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.
