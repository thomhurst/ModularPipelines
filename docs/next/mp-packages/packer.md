# Packer Package

Strongly typed Packer machine-image build commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Packer
```

Required command-line tool: `packer`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Packer`

## Module example[​](#module-example "Direct link to Module example")

```


public class UsePackerModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var packer = context.Tools.Packer;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Packer integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
