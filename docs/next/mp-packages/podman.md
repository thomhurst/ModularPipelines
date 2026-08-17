# Podman Package

Strongly typed Podman container-management commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Podman
```

Required command-line tool: `podman`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Podman`

## Module example[​](#module-example "Direct link to Module example")

```


public class UsePodmanModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var podman = context.Tools.Podman;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Podman integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
