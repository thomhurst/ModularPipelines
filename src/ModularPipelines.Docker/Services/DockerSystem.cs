using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class DockerSystem
{
    public DockerSystem(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Info(DockerSystemInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSystemInfoOptions(), token);
    }

    public async Task<CommandResult> Df(DockerSystemDfOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSystemDfOptions(), token);
    }

    public async Task<CommandResult> Events(DockerSystemEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSystemEventsOptions(), token);
    }

    public async Task<CommandResult> Prune(DockerSystemPruneOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSystemPruneOptions(), token);
    }
}
