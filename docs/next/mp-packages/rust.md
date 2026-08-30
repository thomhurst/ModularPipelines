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
