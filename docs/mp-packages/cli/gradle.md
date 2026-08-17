# gradle CLI reference

`ModularPipelines.Java` provides strongly typed access to the `gradle` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Java
```

Import `ModularPipelines.Java.Extensions`, then resolve the service with `context.Gradle()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Java.Extensions;

using ModularPipelines.Java.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Gradle().Execute(

            new GradleExecuteOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command | Options record         |
| ----------- | ---------------------- |
| `gradle`    | `GradleExecuteOptions` |
