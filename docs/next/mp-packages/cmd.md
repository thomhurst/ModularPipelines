# Command Prompt Package

Helpers for executing Windows Command Prompt commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Cmd
```

Required command-line tool: `cmd`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Cmd`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseCmdModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var cmd = context.Tools.Cmd;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Cmd integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
