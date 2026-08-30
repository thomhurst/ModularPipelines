# Docker Package

Strongly typed Docker container, image, network, volume, and Buildx commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Docker
```

Required command-line tool: `docker`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Docker`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.Docker.Options;



public class UseDockerModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Docker.InfoAsync(

            new DockerInfoOptions(),

            cancellationToken: cancellationToken);

    }

}
```

The package exposes generated options records for its supported CLI commands.
