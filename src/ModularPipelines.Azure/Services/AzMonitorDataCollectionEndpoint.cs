using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "data-collection")]
public class AzMonitorDataCollectionEndpoint
{
    public AzMonitorDataCollectionEndpoint(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMonitorDataCollectionEndpointCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzMonitorDataCollectionEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionEndpointDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzMonitorDataCollectionEndpointListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionEndpointListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMonitorDataCollectionEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionEndpointShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzMonitorDataCollectionEndpointUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionEndpointUpdateOptions(), cancellationToken: token);
    }
}