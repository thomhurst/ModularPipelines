using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class DockerScoutIntegration
{
    public DockerScoutIntegration(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Configure(DockerScoutIntegrationConfigureOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutIntegrationConfigureOptions(), token);
    }

    public async Task<CommandResult> Delete(DockerScoutIntegrationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutIntegrationDeleteOptions(), token);
    }

    public async Task<CommandResult> List(DockerScoutIntegrationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutIntegrationListOptions(), token);
    }
}
