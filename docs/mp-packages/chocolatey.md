# Chocolatey Package

Strongly typed Chocolatey package-management commands.

## Common options[​](#common-options "Direct link to Common options")

The common-option audit uses Chocolatey 2.7.4 help and the official [command reference](https://docs.chocolatey.org/en-us/choco/commands/). `ChocoOptions` supplies the documented default switches, including `Debug`, `Verbose`, `Yes`, `Timeout`, and proxy settings. Common options follow the command and its package operands, such as `choco install example --timeout=60 --yes`. Value-taking options use `=`; `ProxyPassword` remains secret-masked.

Existing command initializers can continue setting inherited properties. Alias lists now produce one property using the first long name, so previously omitted options such as `--yes,--confirm` and `--timeout,--execution-timeout=VALUE` are available. Short aliases remain attached to that property. Root `--version` and help-only options are not common settings; a command's package `Version` stays command-specific. Comma-separated settings such as `ProxyBypassList` remain single string values.

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
