using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class DockerBuildxImagetools
{
    public DockerBuildxImagetools(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(DockerBuildxImagetoolsCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxImagetoolsCreateOptions(), token);
    }

    public async Task<CommandResult> Inspect(DockerBuildxImagetoolsInspectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
