using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class DockerComposeAlpha
{
    public DockerComposeAlpha(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DryRun(DockerComposeAlphaDryRunOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeAlphaDryRunOptions(), token);
    }

    public async Task<CommandResult> Publish(DockerComposeAlphaPublishOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeAlphaPublishOptions(), token);
    }

    public async Task<CommandResult> Scale(DockerComposeAlphaScaleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeAlphaScaleOptions(), token);
    }

    public async Task<CommandResult> Viz(DockerComposeAlphaVizOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeAlphaVizOptions(), token);
    }
}
