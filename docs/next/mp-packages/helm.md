# Helm Package

Strongly typed Helm package-management commands for Kubernetes.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Helm
```

Required command-line tool: `helm`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Helm`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Helm.Options;



public class UseHelmModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Helm.EnvAsync(

            new HelmEnvOptions(),

            cancellationToken: cancellationToken);

    }

}
```

The package exposes generated options records for its supported CLI commands.
