---
title: Microsoft Teams Package
---

# Microsoft Teams Package

Microsoft Teams notification helpers.

## Installation

```shell
dotnet add package ModularPipelines.MicrosoftTeams
```

## Context entry points

Import `ModularPipelines.MicrosoftTeams.Extensions`, then use this service from a module:

- `context.MicrosoftTeams()`

## Module example

```csharp
using ModularPipelines.MicrosoftTeams.Extensions;

public class UseMicrosoftTeamsModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var microsoftTeams = context.MicrosoftTeams();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("MicrosoftTeams integration is ready");
        return None.Value;
    }
}
```
