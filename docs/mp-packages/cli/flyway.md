# flyway CLI reference

`ModularPipelines.Flyway` provides strongly typed access to the `flyway` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Flyway
```

Import `ModularPipelines.Flyway.Extensions`, then resolve the service with `context.Flyway()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Flyway.Extensions;

using ModularPipelines.Flyway.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Flyway().Info(

            new FlywayInfoOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command           | Options record             |
| --------------------- | -------------------------- |
| `flyway baseline`     | `FlywayBaselineOptions`    |
| `flyway check`        | `FlywayCheckOptions`       |
| `flyway clean`        | `FlywayCleanOptions`       |
| `flyway deploy`       | `FlywayDeployOptions`      |
| `flyway info`         | `FlywayInfoOptions`        |
| `flyway init`         | `FlywayInitOptions`        |
| `flyway list-engines` | `FlywayListEnginesOptions` |
| `flyway migrate`      | `FlywayMigrateOptions`     |
| `flyway prepare`      | `FlywayPrepareOptions`     |
| `flyway repair`       | `FlywayRepairOptions`      |
| `flyway snapshot`     | `FlywaySnapshotOptions`    |
| `flyway validate`     | `FlywayValidateOptions`    |
