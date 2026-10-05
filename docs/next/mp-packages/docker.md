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

## Docker client global options[​](#docker-client-global-options "Direct link to Docker client global options")

Every generated Docker command inherits client settings from `DockerOptions`. For example:

```
using ModularPipelines.Docker.Options;



var options = new DockerInfoOptions

{

    Context = "remote",

    Debug = true,

};



await context.Tools.Docker.InfoAsync(options, cancellationToken: cancellationToken);
```

The global settings appear before the subcommand: `docker --context=remote --debug info`. Client configuration, daemon host, logging, and TLS options use the same inherited surface. Help/version actions are not inherited settings.

### Global and command-specific names[​](#global-and-command-specific-names "Direct link to Global and command-specific names")

A command or plugin can use the same option name for a different purpose. Those settings remain independent: `Context` selects the Docker client context, while `BuildxContext` on `DockerBuildxCreateOptions` is the command's context/endpoint operand. Similarly, Buildx-specific debug options remain command options rather than replacing Docker's global `Debug` setting.

In V4, command members that collide with a new global property receive a scoped name based on the command scope. The following mappings apply to this generated Docker snapshot:

| Command scope                                                                                                 | Previous member   | V4 command member |
| ------------------------------------------------------------------------------------------------------------- | ----------------- | ----------------- |
| `docker build`, `docker buildx`                                                                               | `Debug`           | `CliDebug`        |
| Direct `docker buildx` subcommands, including `build`, `create`, `history`, `dap`, `imagetools`, and `policy` | `Debug`           | `BuildxDebug`     |
| `docker buildx dap build`                                                                                     | `Debug`           | `DapDebug`        |
| Direct `docker buildx history` subcommands                                                                    | `Debug`           | `HistoryDebug`    |
| `docker buildx history inspect attachment`                                                                    | `Debug`           | `InspectDebug`    |
| `docker image build`                                                                                          | `Debug`           | `ImageDebug`      |
| `docker buildx imagetools create` and `inspect`                                                               | `Debug`           | `ImageToolsDebug` |
| `docker buildx policy eval` and `test`                                                                        | `Debug`           | `PolicyDebug`     |
| `docker buildx create`                                                                                        | `Context` operand | `BuildxContext`   |
| `docker context create`, `export`, `import`, `inspect`, `rm`, `update`, and `use`                             | `Context` operand | `ContextContext`  |

The operand rename also applies to named constructor arguments and deconstruction. For example, remove the named context with `new DockerContextRmOptions(ContextContext: ["staging"])`. Setting inherited `Context = "staging"` instead selects the Docker client's context; it does not supply the context name to remove.

The [generated Docker reference](/ModularPipelines/docs/next/mp-packages/cli/docker.md) lists each command's options record. Use the scoped members above for command-specific values and the inherited `Context` or `Debug` only for Docker client settings.
