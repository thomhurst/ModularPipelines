# pnpm CLI reference

`ModularPipelines.Node` provides strongly typed access to the `pnpm` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Node
```

Import `ModularPipelines.Node.Extensions`, then resolve the service with `context.Pnpm()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Node.Extensions;

using ModularPipelines.Node.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Pnpm().Unlink(

            new PnpmUnlinkOptions(),

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
