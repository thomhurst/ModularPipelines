# WinGet Package

Strongly typed Windows Package Manager commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.WinGet
```

Required command-line tool: `winget`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Winget`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseWingetModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var winget = context.Tools.Winget;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Winget integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
