# jq CLI reference

`ModularPipelines.Jq` provides strongly typed access to the `jq` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Jq
```

Import `ModularPipelines.Jq.Extensions`, then resolve the service with `context.Jq()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Jq.Extensions;

using ModularPipelines.Jq.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Jq().Execute(

            new JqExecuteOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command | Options record     |
| ----------- | ------------------ |
| `jq`        | `JqExecuteOptions` |
