# go CLI reference

`ModularPipelines.Go` provides strongly typed access to the `go` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Go
```

Import `ModularPipelines.Go.Extensions`, then resolve the service with `context.Go()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Go.Extensions;

using ModularPipelines.Go.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Go().Fix(

            new GoFixOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command   | Options record      |
| ------------- | ------------------- |
| `go build`    | `GoBuildOptions`    |
| `go fix`      | `GoFixOptions`      |
| `go generate` | `GoGenerateOptions` |
| `go test`     | `GoTestOptions`     |
| `go vet`      | `GoVetOptions`      |
