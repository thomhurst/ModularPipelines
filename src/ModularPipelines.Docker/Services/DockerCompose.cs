using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class DockerCompose
{
    public DockerCompose(
        DockerComposeAlpha composeAlpha,
        ICommand internalCommand
    )
    {
        ComposeAlpha = composeAlpha;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public DockerComposeAlpha ComposeAlpha { get; }

    public async Task<CommandResult> Alpha(DockerComposeAlphaOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeAlphaOptions(), token);
    }

    public async Task<CommandResult> Build(DockerComposeBuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeBuildOptions(), token);
    }

    public async Task<CommandResult> Config(DockerComposeConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeConfigOptions(), token);
    }

    public async Task<CommandResult> Cp(DockerComposeCpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(DockerComposeCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeCreateOptions(), token);
    }

    public async Task<CommandResult> Down(DockerComposeDownOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeDownOptions(), token);
    }

    public async Task<CommandResult> Events(DockerComposeEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeEventsOptions(), token);
    }

    public async Task<CommandResult> Exec(DockerComposeExecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Images(DockerComposeImagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeImagesOptions(), token);
    }

    public async Task<CommandResult> Kill(DockerComposeKillOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeKillOptions(), token);
    }

    public async Task<CommandResult> Logs(DockerComposeLogsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeLogsOptions(), token);
    }

    public async Task<CommandResult> Ls(DockerComposeLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeLsOptions(), token);
    }

    public async Task<CommandResult> Pause(DockerComposePauseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposePauseOptions(), token);
    }

    public async Task<CommandResult> Port(DockerComposePortOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Ps(DockerComposePsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposePsOptions(), token);
    }

    public async Task<CommandResult> Pull(DockerComposePullOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposePullOptions(), token);
    }

    public async Task<CommandResult> Push(DockerComposePushOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposePushOptions(), token);
    }

    public async Task<CommandResult> Restart(DockerComposeRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeRestartOptions(), token);
    }

    public async Task<CommandResult> Rm(DockerComposeRmOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeRmOptions(), token);
    }

    public async Task<CommandResult> Run(DockerComposeRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(DockerComposeStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeStartOptions(), token);
    }

    public async Task<CommandResult> Stop(DockerComposeStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeStopOptions(), token);
    }

    public async Task<CommandResult> Top(DockerComposeTopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeTopOptions(), token);
    }

    public async Task<CommandResult> Unpause(DockerComposeUnpauseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeUnpauseOptions(), token);
    }

    public async Task<CommandResult> Up(DockerComposeUpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeUpOptions(), token);
    }

    public async Task<CommandResult> Version(DockerComposeVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeVersionOptions(), token);
    }

    public async Task<CommandResult> Wait(DockerComposeWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeWaitOptions(), token);
    }

    public async Task<CommandResult> Watch(DockerComposeWatchOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeWatchOptions(), token);
    }
}
