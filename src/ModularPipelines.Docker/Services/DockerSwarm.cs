using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class DockerSwarm
{
    public DockerSwarm(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public virtual async Task<CommandResult> Ca(DockerSwarmCaOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmCaOptions(), token);
    }

    public virtual async Task<CommandResult> Init(DockerSwarmInitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmInitOptions(), token);
    }

    public virtual async Task<CommandResult> JoinToken(DockerSwarmJoinTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Join(DockerSwarmJoinOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Leave(DockerSwarmLeaveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmLeaveOptions(), token);
    }

    public virtual async Task<CommandResult> UnlockKey(DockerSwarmUnlockKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmUnlockKeyOptions(), token);
    }

    public virtual async Task<CommandResult> Unlock(DockerSwarmUnlockOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmUnlockOptions(), token);
    }

    public virtual async Task<CommandResult> Update(DockerSwarmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmUpdateOptions(), token);
    }
}
