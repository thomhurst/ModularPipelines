# Microsoft Teams Package

Microsoft Teams notification helpers.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.MicrosoftTeams
```

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.MicrosoftTeams`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseMicrosoftTeamsModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var microsoftTeams = context.Tools.MicrosoftTeams;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("MicrosoftTeams integration is ready");

        return None.Value;

    }

}
```
