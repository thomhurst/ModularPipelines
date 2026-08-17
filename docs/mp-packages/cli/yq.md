# yq CLI reference

`ModularPipelines.Yq` provides strongly typed access to the `yq` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Yq
```

Import `ModularPipelines.Yq.Extensions`, then resolve the service with `context.Yq()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Yq.Extensions;

using ModularPipelines.Yq.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Yq().EvalAll(

            new YqEvalAllOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command   | Options record     |
| ------------- | ------------------ |
| `yq eval`     | `YqEvalOptions`    |
| `yq eval-all` | `YqEvalAllOptions` |
