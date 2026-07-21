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

Import `ModularPipelines.Email.Extensions`, then use this service from a module:

- `context.Email()`

## Module example

```csharp
using ModularPipelines.Email.Extensions;

public class UseEmailModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var email = context.Email();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Email integration is ready");
    }
}
```
