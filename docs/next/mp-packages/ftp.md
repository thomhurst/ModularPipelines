# FTP Package

FTP file-transfer helpers for pipeline modules.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Ftp
```

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Ftp`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseFtpModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var ftp = context.Tools.Ftp;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Ftp integration is ready");

        return None.Value;

    }

}
```
