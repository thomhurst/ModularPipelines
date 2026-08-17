# TeamCity Package

TeamCity environment and build integration helpers.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.TeamCity
```

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.TeamCity`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseTeamCityModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var teamCity = context.Tools.TeamCity;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("TeamCity integration is ready");

        return None.Value;

    }

}
```
