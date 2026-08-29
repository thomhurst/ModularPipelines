# pnpm CLI reference

`ModularPipelines.Node` provides strongly typed access to the `pnpm` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Node
```

Resolve the service with `context.Tools.Pnpm`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Node.Services.IPnpm>()` instead.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Node.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Pnpm.AuditAsync(

            new PnpmAuditOptions

            {

                AuditLevel = "high",

            },

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command    | Options record       |
| -------------- | -------------------- |
| `pnpm add`     | `PnpmAddOptions`     |
| `pnpm audit`   | `PnpmAuditOptions`   |
| `pnpm create`  | `PnpmCreateOptions`  |
| `pnpm dlx`     | `PnpmDlxOptions`     |
| `pnpm init`    | `PnpmInitOptions`    |
| `pnpm publish` | `PnpmPublishOptions` |
| `pnpm run`     | `PnpmRunOptions`     |
| `pnpm stage`   | `PnpmStageOptions`   |
| `pnpm unlink`  | `PnpmUnlinkOptions`  |
| `pnpm why`     | `PnpmWhyOptions`     |
