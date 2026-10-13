# minikube Package

Strongly typed minikube commands for local Kubernetes clusters.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Minikube
```

Required command-line tool: `minikube`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Minikube`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseMinikubeModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var minikube = context.Tools.Minikube;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Minikube integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.

## Shared settings[​](#shared-settings "Direct link to Shared settings")

Command records inherit the persistent settings listed by `minikube options` from `MinikubeOptions`. These include `Profile`, `Bootstrapper`, `User`, `Rootless`, `SkipAudit`, and logging settings. They render before the command path, including for nested commands:

```
await context.Tools.Minikube.Config.ViewAsync(new MinikubeConfigViewOptions

{

    Profile = "ci",

    V = 0,

}, cancellationToken: cancellationToken);
```

This renders `minikube --profile=ci --v=0 config view`. A zero verbosity value is preserved. Boolean settings use `CliOptionValue`: assign `"true"` or `"false"` for explicit values, `CliOptionValue.Bare` for a bare flag, or `null` to leave a setting unspecified. For example, `Logtostderr = "false"` overrides its true default. Logging severity thresholds accept names such as `WARNING` or numeric strings, and the log-file size limit supports the CLI's unsigned 64-bit range. Profile and logging values are scalar settings, not repeated lists.

Command-local settings retain their scope: for example, `Driver` belongs to `MinikubeStartOptions` and `Output` belongs to `MinikubeStatusOptions`. Root help controls are not inherited configuration settings.

### Migration[​](#migration "Direct link to Migration")

Replace manually supplied persistent flags in `Arguments` with the corresponding typed properties. Do not supply the same setting through both mechanisms. Code reflecting over a command record with `BindingFlags.DeclaredOnly` must also inspect `MinikubeOptions` to discover inherited settings. Existing command and version APIs are unchanged by this scope audit.
