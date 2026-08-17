# Azure Package

Azure SDK helpers and strongly typed Azure CLI commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Azure
```

Required command-line tool: `az`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Azure`
* `context.Tools.Az`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Azure.Options;



public class UseAzureModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Az.Account.ListAsync(

            new AzAccountListOptions(),

            cancellationToken: cancellationToken);

    }

}
```

The package exposes generated options records for its supported CLI commands.
