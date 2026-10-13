# Podman Package

Strongly typed Podman container-management commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Podman
```

Required command-line tool: `podman`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Podman`

## Module example[​](#module-example "Direct link to Module example")

```


public class UsePodmanModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var podman = context.Tools.Podman;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Podman integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.

## Root execution settings[​](#root-execution-settings "Direct link to Root execution settings")

Every command options record inherits Podman's public root settings from `PodmanOptions`. Set `Connection` and `Remote` for a remote service, or `Root`, `Runroot`, `Runtime`, and `StorageOpt` for local storage and runtime selection. These settings render before the subcommand, including nested commands.

```
var options = new PodmanPsOptions

{

    Connection = "builder",

    Remote = CliOptionValue.Bare,

    Syslog = "false",

};

// podman --connection=builder --remote --syslog=false ps
```

The optional boolean properties accept `CliOptionValue.Bare`, `"true"`, `"false"`, or `null` to omit the switch. `CdiSpecDir`, `HooksDir`, `Module`, `RuntimeFlag`, and `StorageOpt` accept collections and repeat the switch for each value. Short aliases `-c` and `-r` remain available in option metadata. Identity and TLS key settings refer to file paths; they are not credential contents.

Compose provider options such as `ProjectName`, `File`, and `Profile` stay on `PodmanComposeOptions`. A command-local `--identity` or TLS setting configures that command's target and remains independent of the root connection settings.

### V4 migration[​](#v4-migration "Direct link to V4 migration")

Root execution settings are now available through every options record. Where a command already had an option with the same name, its generated property receives a command-specific name. In particular, use inherited `Identity` for the active connection and `ConnectionIdentity` on `PodmanSystemConnectionAddOptions` for the connection being added. Its local TLS properties are `ConnectionTlsCa`, `ConnectionTlsCert`, and `ConnectionTlsKey`; the root properties remain `TlsCa`, `TlsCert`, and `TlsKey`. `PodmanSystemServiceOptions` uses `SystemTlsCert` and `SystemTlsKey` for its serving certificate.

Build-specific hooks, runtime flags, and SSH options use scoped names such as `CliHooksDir`, `CliRuntimeFlag`, and `CliSsh` on `PodmanBuildOptions`, with `Image` and `Farm` prefixes on the corresponding nested build records. Commit commands use `CliConfig` or `ContainerConfig` for their local configuration. Compose build uses `ComposeSsh` for its provider setting. Do not move Compose provider settings onto `PodmanOptions`.

This surface was audited against Podman 6.1.3 and Docker Compose 5.6.0 help.
