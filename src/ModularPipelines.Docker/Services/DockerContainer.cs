using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class DockerContainer
{
    public DockerContainer(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public virtual async Task<CommandResult> Attach(DockerContainerAttachOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Commit(DockerContainerCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Cp(DockerContainerCpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Create(DockerContainerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Diff(DockerContainerDiffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerDiffOptions(), token);
    }

    public virtual async Task<CommandResult> Exec(DockerContainerExecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Export(DockerContainerExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Inspect(DockerContainerInspectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Kill(DockerContainerKillOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Logs(DockerContainerLogsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Ls(DockerContainerLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerLsOptions(), token);
    }

    public virtual async Task<CommandResult> Pause(DockerContainerPauseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerPauseOptions(), token);
    }

    public virtual async Task<CommandResult> Port(DockerContainerPortOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerPortOptions(), token);
    }

    public virtual async Task<CommandResult> Prune(DockerContainerPruneOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerPruneOptions(), token);
    }

    public virtual async Task<CommandResult> Rename(DockerContainerRenameOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerRenameOptions(), token);
    }

    public virtual async Task<CommandResult> Restart(DockerContainerRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Rm(DockerContainerRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Run(DockerContainerRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Start(DockerContainerStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Stats(DockerContainerStatsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerStatsOptions(), token);
    }

    public virtual async Task<CommandResult> Stop(DockerContainerStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Top(DockerContainerTopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Unpause(DockerContainerUnpauseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerUnpauseOptions(), token);
    }

    public virtual async Task<CommandResult> Update(DockerContainerUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Wait(DockerContainerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerWaitOptions(), token);
    }
}
