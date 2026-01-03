using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb")]
public class AzCosmosdbPrivateEndpointConnection
{
    public AzCosmosdbPrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Approve(AzCosmosdbPrivateEndpointConnectionApproveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPrivateEndpointConnectionApproveOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbPrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPrivateEndpointConnectionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Reject(AzCosmosdbPrivateEndpointConnectionRejectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPrivateEndpointConnectionRejectOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzCosmosdbPrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPrivateEndpointConnectionShowOptions(), cancellationToken: token);
    }
}