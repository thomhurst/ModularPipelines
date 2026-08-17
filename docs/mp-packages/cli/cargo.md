# cargo CLI reference

`ModularPipelines.Rust` provides strongly typed access to the `cargo` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Rust
```

Import `ModularPipelines.Rust.Extensions`, then resolve the service with `context.Cargo()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Rust.Extensions;

using ModularPipelines.Rust.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Cargo().Build(

            new CargoBuildOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command       | Options record          |
| ----------------- | ----------------------- |
| `cargo add`       | `CargoAddOptions`       |
| `cargo bench`     | `CargoBenchOptions`     |
| `cargo build`     | `CargoBuildOptions`     |
| `cargo check`     | `CargoCheckOptions`     |
| `cargo clean`     | `CargoCleanOptions`     |
| `cargo doc`       | `CargoDocOptions`       |
| `cargo init`      | `CargoInitOptions`      |
| `cargo install`   | `CargoInstallOptions`   |
| `cargo new`       | `CargoNewOptions`       |
| `cargo publish`   | `CargoPublishOptions`   |
| `cargo remove`    | `CargoRemoveOptions`    |
| `cargo run`       | `CargoRunOptions`       |
| `cargo search`    | `CargoSearchOptions`    |
| `cargo test`      | `CargoTestOptions`      |
| `cargo uninstall` | `CargoUninstallOptions` |
| `cargo update`    | `CargoUpdateOptions`    |
