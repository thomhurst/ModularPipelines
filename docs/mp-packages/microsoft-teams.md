# Microsoft Teams Package

Microsoft Teams notification helpers.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.MicrosoftTeams
```

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.MicrosoftTeams.Extensions`, then use this service from a module:

* `context.MicrosoftTeams()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.MicrosoftTeams.Extensions;



public class UseMicrosoftTeamsModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var microsoftTeams = context.MicrosoftTeams();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("MicrosoftTeams integration is ready");

    }

}
```
