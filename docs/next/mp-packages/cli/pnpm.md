# pnpm CLI reference

`ModularPipelines.Node` provides strongly typed access to the `pnpm` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `pnpm` executable. Install it separately and ensure `pnpm` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Node
```

Resolve the service with `context.Tools.Pnpm`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Node.Services.IPnpm>()` instead.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.Node.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Pnpm.AuditAsync(

            new PnpmAuditOptions()

            {

                AuditLevel = "high",

            },

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command             | Options record               |
| ----------------------- | ---------------------------- |
| `pnpm add`              | `PnpmAddOptions`             |
| `pnpm audit`            | `PnpmAuditOptions`           |
| `pnpm audit signatures` | `PnpmAuditSignaturesOptions` |
| `pnpm create`           | `PnpmCreateOptions`          |
| `pnpm dlx`              | `PnpmDlxOptions`             |
| `pnpm init`             | `PnpmInitOptions`            |
| `pnpm publish`          | `PnpmPublishOptions`         |
| `pnpm run`              | `PnpmRunOptions`             |
| `pnpm stage`            | `PnpmStageOptions`           |
| `pnpm stage approve`    | `PnpmStageApproveOptions`    |
| `pnpm stage download`   | `PnpmStageDownloadOptions`   |
| `pnpm stage list`       | `PnpmStageListOptions`       |
| `pnpm stage publish`    | `PnpmStagePublishOptions`    |
| `pnpm stage reject`     | `PnpmStageRejectOptions`     |
| `pnpm stage view`       | `PnpmStageViewOptions`       |
| `pnpm unlink`           | `PnpmUnlinkOptions`          |
| `pnpm why`              | `PnpmWhyOptions`             |
