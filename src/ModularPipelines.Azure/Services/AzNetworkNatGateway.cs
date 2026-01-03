using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "nat")]
public class AzNetworkNatGateway
{
    public AzNetworkNatGateway(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkNatGatewayCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkNatGatewayDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNatGatewayDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkNatGatewayListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNatGatewayListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkNatGatewayShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNatGatewayShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkNatGatewayUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNatGatewayUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkNatGatewayWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNatGatewayWaitOptions(), cancellationToken: token);
    }
}