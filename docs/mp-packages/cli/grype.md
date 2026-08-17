# grype CLI reference

`ModularPipelines.Grype` provides strongly typed access to the `grype` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Grype
```

Import `ModularPipelines.Grype.Extensions`, then resolve the service with `context.Grype()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Grype.Extensions;

using ModularPipelines.Grype.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Grype().Explain(

            new GrypeExplainOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command            | Options record             |
| ---------------------- | -------------------------- |
| `grype db check`       | `GrypeDbCheckOptions`      |
| `grype db delete`      | `GrypeDbDeleteOptions`     |
| `grype db diff`        | `GrypeDbDiffOptions`       |
| `grype db import`      | `GrypeDbImportOptions`     |
| `grype db list`        | `GrypeDbListOptions`       |
| `grype db providers`   | `GrypeDbProvidersOptions`  |
| `grype db search`      | `GrypeDbSearchOptions`     |
| `grype db search vuln` | `GrypeDbSearchVulnOptions` |
| `grype db status`      | `GrypeDbStatusOptions`     |
| `grype db update`      | `GrypeDbUpdateOptions`     |
| `grype explain`        | `GrypeExplainOptions`      |
