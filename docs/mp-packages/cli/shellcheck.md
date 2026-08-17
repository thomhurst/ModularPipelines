# shellcheck CLI reference

`ModularPipelines.Shellcheck` provides strongly typed access to the `shellcheck` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Shellcheck
```

Import `ModularPipelines.Shellcheck.Extensions`, then resolve the service with `context.Shellcheck()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Shellcheck.Extensions;

using ModularPipelines.Shellcheck.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Shellcheck().Execute(

            new ShellcheckExecuteOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command  | Options record             |
| ------------ | -------------------------- |
| `shellcheck` | `ShellcheckExecuteOptions` |
