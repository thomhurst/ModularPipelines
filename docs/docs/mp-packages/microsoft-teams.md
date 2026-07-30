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

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.MicrosoftTeams`

## Module example

```csharp

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
