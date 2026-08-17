# minikube Package

Strongly typed minikube commands for local Kubernetes clusters.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Minikube
```

Required command-line tool: `minikube`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Minikube`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseMinikubeModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var minikube = context.Tools.Minikube;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Minikube integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
