# Grype Package

Strongly typed Grype vulnerability-scanning commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Grype
```

Required command-line tool: `grype`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Grype`

## Module example[​](#module-example "Direct link to Module example")

```


public class UseGrypeModule : SyncModule<None>

{

    protected override None Execute(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        var grype = context.Tools.Grype;



        // Call the integration's strongly typed operations here.

        context.Logger.LogInformation("Grype integration is ready");

        return None.Value;

    }

}
```

The package exposes generated options records for its supported CLI commands.

## Persistent settings[​](#persistent-settings "Direct link to Persistent settings")

Audited against Grype 0.120.0. Every generated command inherits `Config`, `Profile`, `Quiet`, and `Verbose` from `GrypeOptions`. They render before the command path. Configuration files and profiles are collections: each value becomes a separate switch occurrence, with spaces and commas preserved inside that value. `Verbose` is a nullable count; `0` is emitted explicitly, while an unset value is omitted. `Quiet = false` and `Quiet = null` omit the flag.

```
using ModularPipelines.Grype.Options;



await context.Tools.Grype.Db.StatusAsync(new GrypeDbStatusOptions

{

    Config = ["base config.yaml", "production.yaml"],

    Profile = ["production"],

    Verbose = 1,

    Output = "json",

}, cancellationToken: cancellationToken);
```

This emits global settings before `db status`, followed by its local `--output=json`. Root scan flags such as `--scope`, `--fail-on`, and the scan `--output` are not persistent settings. The database status command's `Output` remains its own scalar option. See the [generated command reference](/ModularPipelines/docs/next/mp-packages/cli/grype.md) for complete command-specific options.

### Migration[​](#migration "Direct link to Migration")

Existing initializers keep the same property names and types. The four shared properties now belong to `GrypeOptions`; reflection over command records must include inherited properties rather than using `BindingFlags.DeclaredOnly`. Derived overrides still emit one switch. No command or version API is added or removed by this inheritance change.
