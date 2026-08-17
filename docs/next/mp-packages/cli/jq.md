# jq CLI reference

`ModularPipelines.Jq` provides strongly typed access to the `jq` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `jq` executable. Install it separately and ensure `jq` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Jq
```

Resolve the service with `context.Tools.Jq`. For projects older than C# 14, import `ModularPipelines.Jq.Extensions` and use the `context.Jq()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Jq.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Jq.ExecuteAsync(

            new JqExecuteOptions()

            {

                Filter = ".",

                InputFiles = ["input.json"],

            },

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command | Options record     |
| ----------- | ------------------ |
| `jq`        | `JqExecuteOptions` |
