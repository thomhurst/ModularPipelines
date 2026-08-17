# sonar-scanner CLI reference

`ModularPipelines.SonarScanner` provides strongly typed access to the `sonar-scanner` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.SonarScanner
```

Import `ModularPipelines.SonarScanner.Extensions`, then resolve the service with `context.SonarScanner()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.SonarScanner.Extensions;

using ModularPipelines.SonarScanner.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.SonarScanner().Execute(

            new SonarScannerExecuteOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command     | Options record               |
| --------------- | ---------------------------- |
| `sonar-scanner` | `SonarScannerExecuteOptions` |
