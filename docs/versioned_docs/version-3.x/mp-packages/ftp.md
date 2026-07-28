---
title: FTP Package
---

# FTP Package

FTP file-transfer helpers for pipeline modules.

## Installation

```shell
dotnet add package ModularPipelines.Ftp
```

## Context entry points

Import `ModularPipelines.Ftp.Extensions`, then use this service from a module:

- `context.Ftp()`

## Module example

```csharp
using ModularPipelines.Ftp.Extensions;

public class UseFtpModule : SyncModule
{
    protected override void ExecuteModule(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var ftp = context.Ftp();

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Ftp integration is ready");
    }
}
```
