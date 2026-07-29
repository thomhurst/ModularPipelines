---
title: Slack Package
---

# Slack Package

Slack notification helpers.

## Installation

```shell
dotnet add package ModularPipelines.Slack
```

## Context entry points

Import `ModularPipelines.Slack.Extensions`, then use this service from a module:

- `context.Slack()`

## Module example

```csharp
using ModularPipelines.Slack.Extensions;

public class UseSlackModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var slack = context.Slack();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Slack integration is ready");
        return None.Value;
    }
}
```
