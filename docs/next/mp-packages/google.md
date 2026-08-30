# Google Cloud Package

Strongly typed Google Cloud CLI commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Google
```

Required command-line tool: `gcloud`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Gcloud`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.Google.Options;



public class UseGcloudModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Gcloud.InfoAsync(

            new GcloudInfoOptions

            {

                Anonymize = true,

            },

            cancellationToken: cancellationToken);

    }

}
```

The package exposes generated options records for its supported CLI commands.
