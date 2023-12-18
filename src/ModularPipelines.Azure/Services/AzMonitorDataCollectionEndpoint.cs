using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "data-collection")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorDataCollectionEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionEndpointDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMonitorDataCollectionEndpointListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionEndpointListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMonitorDataCollectionEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionEndpointShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorDataCollectionEndpointUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionEndpointUpdateOptions(), token);
    }
}