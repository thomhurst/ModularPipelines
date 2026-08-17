# Vault Package

Strongly typed HashiCorp Vault commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Vault
```

Required command-line tool: `vault`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Vault`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseVaultModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var vault = context.Tools.Vault;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Vault integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.
