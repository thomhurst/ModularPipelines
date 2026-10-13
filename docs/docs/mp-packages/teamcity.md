---
title: TeamCity Package
---

# TeamCity Package

TeamCity environment and build integration helpers.

## Installation

```shell
dotnet add package ModularPipelines.TeamCity
```

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.TeamCity`

## Module example

```csharp

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


## V4 migration

`TeamCityExtensions` now uses the `ModularPipelines.TeamCity` namespace. Replace imports
of `ModularPipelines.TeamCity.Extensions` with the package root namespace.
`RegisterTeamCityContext` remains public for generated registration but is hidden from
IntelliSense. Module code uses `context.Tools.TeamCity`; environment values remain available
through `EnvironmentVariables`.
