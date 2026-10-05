---
title: FTP Package
---

# FTP Package

FTP file-transfer helpers for pipeline modules.

## Installation

```shell
dotnet add package ModularPipelines.Ftp
```

## Connect from a module

Import `ModularPipelines.Ftp` for the options and use `context.Tools.Ftp`:

```csharp
using System.Net;
using ModularPipelines.Ftp;

public class ConnectFtpModule : Module<None>
{
    protected override async Task<None> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var options = new FtpOptions(
            "ftp.example.com",
            new NetworkCredential(
                Environment.GetEnvironmentVariable("FTP_USERNAME"),
                Environment.GetEnvironmentVariable("FTP_PASSWORD")))
        {
            RequireEncryption = true,
        };

        var client = await context.Tools.Ftp.GetFtpClientAsync(options, cancellationToken);
        context.Logger.LogInformation("Connected to {Host}", client.Host);
        return None.Value;
    }
}
```

The cancellation token applies to connection establishment. Pass a cancellation token
separately to subsequent FluentFTP operations. The FTP context disposes connected clients
when its scope ends; failed or canceled setup disposes the client before returning the error.
A token canceled before the call prevents client configuration and connection attempts.

Set `RequireEncryption = true` when sending credentials. This requires explicit FTPS
and fails before authentication if the server rejects TLS. To use implicit FTPS, set
`client.Config.EncryptionMode = FtpEncryptionMode.Implicit` in `ClientConfigurator`.
Both control and data connections remain encrypted. The default is `false` for
compatibility with plaintext FTP servers; that mode uses FluentFTP auto-detection
and can send credentials without encryption.

## V4 migration

Replace `using ModularPipelines.Ftp.Options` and `using ModularPipelines.Ftp.Extensions`
with `using ModularPipelines.Ftp`. `GetFtpClientAsync` accepts an optional
`CancellationToken`; custom `IFtp` implementations must update their signature.
`RegisterFtpContext` remains public for generated registration but is hidden from
IntelliSense. Module code uses `context.Tools.Ftp` without calling registration plumbing.
