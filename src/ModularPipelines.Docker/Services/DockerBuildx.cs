using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class DockerBuildx
{
    public DockerBuildx(
        DockerBuildxImagetools buildxImagetools,
        ICommand internalCommand
    )
    {
        BuildxImagetools = buildxImagetools;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public DockerBuildxImagetools BuildxImagetools { get; }

    public async Task<CommandResult> Bake(DockerBuildxBakeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxBakeOptions(), token);
    }

    public async Task<CommandResult> Build(DockerBuildxBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(DockerBuildxCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxCreateOptions(), token);
    }

    public async Task<CommandResult> Debug(DockerBuildxDebugOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxDebugOptions(), token);
    }

    public async Task<CommandResult> DebugBuild(DockerBuildxDebugBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Du(DockerBuildxDuOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxDuOptions(), token);
    }

    public async Task<CommandResult> Imagetools(DockerBuildxImagetoolsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxImagetoolsOptions(), token);
    }

    public async Task<CommandResult> Inspect(DockerBuildxInspectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxInspectOptions(), token);
    }

    public async Task<CommandResult> Ls(DockerBuildxLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxLsOptions(), token);
    }

    public async Task<CommandResult> Prune(DockerBuildxPruneOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxPruneOptions(), token);
    }

    public async Task<CommandResult> Rm(DockerBuildxRmOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxRmOptions(), token);
    }

    public async Task<CommandResult> Stop(DockerBuildxStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxStopOptions(), token);
    }

    public async Task<CommandResult> Use(DockerBuildxUseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Version(DockerBuildxVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxVersionOptions(), token);
    }
}
