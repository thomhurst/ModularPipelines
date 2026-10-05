# Rust Package

Strongly typed Cargo commands for Rust projects.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Rust
```

Required command-line tool: `cargo`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Cargo`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.Rust.Options;



public class UseCargoModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Cargo.CheckAsync(

            new CargoCheckOptions

            {

                Quiet = true,

            },

            cancellationToken: cancellationToken);

    }

}
```

The package exposes generated options records for its supported CLI commands.

## Common Cargo settings[​](#common-cargo-settings "Direct link to Common Cargo settings")

All command records inherit `Verbose`, `Quiet`, `Color`, `Config`, `Locked`, `Offline`, and `Frozen` from `CargoOptions`. These settings render before the command name. `Verbose = 2` emits `--verbose --verbose`; `Config` emits a separate `--config` argument for each TOML override or configuration-file path. Configuration values are treated as secrets because overrides can contain registry credentials.

```
new CargoBuildOptions

{

    Verbose = 2,

    Config = ["build.jobs=2", "local configuration.toml"],

    Offline = true,

    Release = true,

};
```

This renders the common settings before `build` and the command-specific `--release` after it. Each configuration entry remains one argument, including paths with spaces. Boolean settings set to `false` or `null` are omitted.

`ManifestPath`, package selection, compilation settings, and nightly `Z` switches remain command-specific. Cargo's nightly-only `-C` and rustup's `+toolchain` selector are not unconditional global properties. Use `WorkingDirectory` for process directory selection; select a toolchain through the executable or rustup environment. See the [Cargo command reference](https://doc.rust-lang.org/cargo/commands/cargo.html) for nightly prerequisites and configuration semantics.

When upgrading, replace command-specific color enum names, such as `CargoBuildColor`, with the shared `CargoColor` enum.
