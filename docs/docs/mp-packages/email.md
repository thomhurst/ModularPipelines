---
title: Email Package
---

# Email Package

Email delivery helpers for pipeline notifications.

## Installation

```shell
dotnet add package ModularPipelines.Email
```

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Email`

## Module example

```csharp

public class UseEmailModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var email = context.Tools.Email;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Email integration is ready");
        return None.Value;
    }
}
```
