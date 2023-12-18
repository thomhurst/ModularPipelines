using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb")]
public class AzNetworkLbAddressPool
{
    public AzNetworkLbAddressPool(
        AzNetworkLbAddressPoolAddress address,
        AzNetworkLbAddressPoolTunnelInterface tunnelInterface,
        ICommand internalCommand
    )
    {
        Address = address;
        TunnelInterface = tunnelInterface;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkLbAddressPoolAddress Address { get; }

    public AzNetworkLbAddressPoolTunnelInterface TunnelInterface { get; }

    public async Task<CommandResult> Create(AzNetworkLbAddressPoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkLbAddressPoolDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkLbAddressPoolListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkLbAddressPoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkLbAddressPoolShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkLbAddressPoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkLbAddressPoolUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkLbAddressPoolWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkLbAddressPoolWaitOptions(), token);
    }
}