# cargo CLI reference

`ModularPipelines.Rust` provides strongly typed access to the `cargo` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `cargo` executable. Install it separately and ensure `cargo` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Rust
```

Resolve the service with `context.Tools.Cargo`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Rust.Services.ICargo>()` instead.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.Rust.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Cargo.CheckAsync(

            new CargoCheckOptions()

            {

                Quiet = true,

            },

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
