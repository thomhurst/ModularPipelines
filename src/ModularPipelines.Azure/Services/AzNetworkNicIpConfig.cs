using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nic")]
public class AzNetworkNicIpConfig
{
    public AzNetworkNicIpConfig(
        AzNetworkNicIpConfigAddressPool addressPool,
        AzNetworkNicIpConfigInboundNatRule inboundNatRule,
        ICommand internalCommand
    )
    {
        AddressPool = addressPool;
        InboundNatRule = inboundNatRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkNicIpConfigAddressPool AddressPool { get; }

    public AzNetworkNicIpConfigInboundNatRule InboundNatRule { get; }

    public async Task<CommandResult> Create(AzNetworkNicIpConfigCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkNicIpConfigDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkNicIpConfigListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkNicIpConfigShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzNetworkNicIpConfigUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetworkNicIpConfigWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNicIpConfigWaitOptions(), token);
    }
}