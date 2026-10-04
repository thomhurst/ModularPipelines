---
title: Docker Package
---

# Docker Package

Strongly typed Docker container, image, network, volume, and Buildx commands.

## Installation

```shell
dotnet add package ModularPipelines.Docker
```

Required command-line tool: `docker`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Docker`

## Module example

```csharp
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

## Docker client global options

Every generated Docker command inherits client settings from `DockerOptions`.
For example:

```csharp
using ModularPipelines.Docker.Options;

var options = new DockerInfoOptions
{
    Context = "remote",
    Debug = true,
};

await context.Tools.Docker.InfoAsync(options, cancellationToken: cancellationToken);
```

The global settings appear before the subcommand:
`docker --context=remote --debug info`. Client configuration, daemon host,
logging, and TLS options use the same inherited surface. Help/version actions
are not inherited settings.

### Global and command-specific names

A command or plugin can use the same option name for a different purpose. Those
settings remain independent: `Context` selects the Docker client context, while
`BuildxContext` on `DockerBuildxCreateOptions` is the command's context/endpoint
operand. Similarly, Buildx-specific debug options remain command options rather
than replacing Docker's global `Debug` setting.

In V4, command members that collide with a new global property receive a scoped
name such as `BuildxContext` or `BuildxDebug`. Update those callers using the
[generated Docker reference](cli/docker.md). Do not move a command-specific value
to the global property merely because its former C# name was the same.
