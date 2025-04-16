using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class DockerVolume
{
    public DockerVolume(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public virtual async Task<CommandResult> Create(DockerVolumeCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerVolumeCreateOptions(), token);
    }

    public virtual async Task<CommandResult> Inspect(DockerVolumeInspectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Ls(DockerVolumeLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerVolumeLsOptions(), token);
    }

    public virtual async Task<CommandResult> Prune(DockerVolumePruneOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerVolumePruneOptions(), token);
    }

    public virtual async Task<CommandResult> Rm(DockerVolumeRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Update(DockerVolumeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerVolumeUpdateOptions(), token);
    }
}
