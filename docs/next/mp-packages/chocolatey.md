# Chocolatey Package

Strongly typed Chocolatey package-management commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Chocolatey
```

Required command-line tool: `choco`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Choco`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseChocoModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var choco = context.Tools.Choco;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Choco integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
