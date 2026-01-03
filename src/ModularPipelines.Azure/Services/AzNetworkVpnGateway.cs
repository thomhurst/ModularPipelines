using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkVpnGatewayDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkVpnGatewayListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkVpnGatewayShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkVpnGatewayUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkVpnGatewayWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnGatewayWaitOptions(), cancellationToken: token);
    }
}