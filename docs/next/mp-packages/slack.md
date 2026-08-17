# Slack Package

Slack notification helpers.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Slack
```

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Slack`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseSlackModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var slack = context.Tools.Slack;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Slack integration is ready");

        return None.Value;

    }

}
```
