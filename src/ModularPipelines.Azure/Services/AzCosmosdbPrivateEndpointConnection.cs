using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPrivateEndpointConnectionApproveOptions(), token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbPrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPrivateEndpointConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> Reject(AzCosmosdbPrivateEndpointConnectionRejectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPrivateEndpointConnectionRejectOptions(), token);
    }

    public async Task<CommandResult> Show(AzCosmosdbPrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPrivateEndpointConnectionShowOptions(), token);
    }
}