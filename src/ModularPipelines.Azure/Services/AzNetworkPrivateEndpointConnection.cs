using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkPrivateEndpointConnection
{
    public AzNetworkPrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Approve(AzNetworkPrivateEndpointConnectionApproveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointConnectionApproveOptions(), token);
    }

    public async Task<CommandResult> Delete(AzNetworkPrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkPrivateEndpointConnectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointConnectionListOptions(), token);
    }

    public async Task<CommandResult> Reject(AzNetworkPrivateEndpointConnectionRejectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointConnectionRejectOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkPrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointConnectionShowOptions(), token);
    }
}

