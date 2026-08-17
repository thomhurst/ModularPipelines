# syft CLI reference

`ModularPipelines.Syft` provides strongly typed access to the `syft` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Syft
```

Import `ModularPipelines.Syft.Extensions`, then resolve the service with `context.Syft()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Syft.Extensions;

using ModularPipelines.Syft.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Syft().Attest(

            new SyftAttestOptions("value"),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command             | Options record               |
| ----------------------- | ---------------------------- |
| `syft attest`           | `SyftAttestOptions`          |
| `syft cataloger list`   | `SyftCatalogerListOptions`   |
| `syft config locations` | `SyftConfigLocationsOptions` |
| `syft convert`          | `SyftConvertOptions`         |
| `syft login`            | `SyftLoginOptions`           |
| `syft scan`             | `SyftScanOptions`            |
