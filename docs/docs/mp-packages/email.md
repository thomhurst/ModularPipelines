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

Import `ModularPipelines.Email` for `EmailSendOptions` and use the discoverable
`context.Tools` surface from a module:

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

## V4 migration

Replace `using ModularPipelines.Email.Options` and `using ModularPipelines.Email.Extensions`
with `using ModularPipelines.Email`. `EmailSendOptions`, `EmailExtensions`, and `IEmail`
now share the package root namespace. `SendAsync` retains its cancellation token and
SMTP behavior.

`RegisterEmailContext` remains public for generated registration but is hidden from
IntelliSense. Module code uses `context.Tools.Email` without calling registration plumbing.
