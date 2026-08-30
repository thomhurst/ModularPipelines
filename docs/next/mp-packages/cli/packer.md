# packer CLI reference

`ModularPipelines.Packer` provides strongly typed access to the `packer` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Packer
```

Resolve the service with `context.Tools.Packer`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Packer.Services.IPacker>()` instead.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.Packer.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Packer.ConsoleAsync(

            new PackerConsoleOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command           | Options record             |
| --------------------- | -------------------------- |
| `packer build`        | `PackerBuildOptions`       |
| `packer console`      | `PackerConsoleOptions`     |
| `packer fix`          | `PackerFixOptions`         |
| `packer fmt`          | `PackerFmtOptions`         |
| `packer hcl2_upgrade` | `PackerHcl2UpgradeOptions` |
| `packer init`         | `PackerInitOptions`        |
| `packer inspect`      | `PackerInspectOptions`     |
| `packer plugins`      | `PackerPluginsOptions`     |
| `packer validate`     | `PackerValidateOptions`    |
