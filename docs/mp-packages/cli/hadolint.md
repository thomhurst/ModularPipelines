# hadolint CLI reference

`ModularPipelines.Hadolint` provides strongly typed access to the `hadolint` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Hadolint
```

Import `ModularPipelines.Hadolint.Extensions`, then resolve the service with `context.Hadolint()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Hadolint.Extensions;

using ModularPipelines.Hadolint.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Hadolint().Execute(

            new HadolintExecuteOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command | Options record           |
| ----------- | ------------------------ |
| `hadolint`  | `HadolintExecuteOptions` |
