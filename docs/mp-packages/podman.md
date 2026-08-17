# Podman Package

Strongly typed Podman container-management commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Podman
```

Required command-line tool: `podman`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.Podman.Extensions`, then use this service from a module:

* `context.Podman()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Podman.Extensions;



public class UsePodmanModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var podman = context.Podman();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Podman integration is ready");

    }

}
```

The package exposes generated options records for its supported CLI commands.
