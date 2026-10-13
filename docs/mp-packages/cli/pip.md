# pip CLI reference

`ModularPipelines.Python` provides strongly typed access to the `pip` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `pip` executable. Install it separately and ensure `pip` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Python
```

Resolve the service with `context.Tools.Pip`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Python.Services.IPip>()` instead.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.Python.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Pip.FreezeAsync(

            new PipFreezeOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command     | Options record        |
| --------------- | --------------------- |
| `pip cache`     | `PipCacheOptions`     |
| `pip check`     | `PipCheckOptions`     |
| `pip config`    | `PipConfigOptions`    |
| `pip download`  | `PipDownloadOptions`  |
| `pip freeze`    | `PipFreezeOptions`    |
| `pip hash`      | `PipHashOptions`      |
| `pip index`     | `PipIndexOptions`     |
| `pip inspect`   | `PipInspectOptions`   |
| `pip install`   | `PipInstallOptions`   |
| `pip list`      | `PipListOptions`      |
| `pip search`    | `PipSearchOptions`    |
| `pip show`      | `PipShowOptions`      |
| `pip uninstall` | `PipUninstallOptions` |
| `pip wheel`     | `PipWheelOptions`     |
