using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkVpnGateway
{
    public AzNetworkVpnGateway(
        AzNetworkVpnGatewayConnection connection,
        ICommand internalCommand
    )
    {
        Connection = connection;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVpnGatewayConnection Connection { get; }

    public async Task<CommandResult> Create(AzNetworkVpnGatewayCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkVpnGatewayDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkVpnGatewayListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkVpnGatewayShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkVpnGatewayUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkVpnGatewayWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayWaitOptions(), token);
    }
}