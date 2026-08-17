# go CLI reference

`ModularPipelines.Go` provides strongly typed access to the `go` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Go
```

Resolve the service with `context.Tools.Go`. For projects older than C# 14, import `ModularPipelines.Go.Extensions` and use the `context.Go()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Go.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Go.VetAsync(

            new GoVetOptions(),

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
