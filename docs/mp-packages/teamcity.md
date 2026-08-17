# TeamCity Package

TeamCity environment and build integration helpers.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.TeamCity
```

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Import `ModularPipelines.TeamCity.Extensions`, then use this service from a module:

* `context.TeamCity()`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.TeamCity.Extensions;



public class UseTeamCityModule : SyncModule

{

    protected override void ExecuteModule(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var teamCity = context.TeamCity();



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("TeamCity integration is ready");

    }

}
```
