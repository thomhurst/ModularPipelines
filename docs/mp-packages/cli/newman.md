# newman CLI reference

`ModularPipelines.Newman` provides strongly typed access to the `newman` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Newman
```

Import `ModularPipelines.Newman.Extensions`, then resolve the service with `context.Newman()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Newman.Extensions;

using ModularPipelines.Newman.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Newman().Url(

            new NewmanUrlOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command  | Options record     |
| ------------ | ------------------ |
| `newman run` | `NewmanRunOptions` |
| `newman URL` | `NewmanUrlOptions` |
