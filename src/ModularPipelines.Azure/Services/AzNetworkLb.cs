using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkLb
{
    public AzNetworkLb(
        AzNetworkLbAddressPool addressPool,
        AzNetworkLbFrontendIp frontendIp,
        AzNetworkLbInboundNatPool inboundNatPool,
        AzNetworkLbInboundNatRule inboundNatRule,
        AzNetworkLbOutboundRule outboundRule,
        AzNetworkLbProbe probe,
        AzNetworkLbRule rule,
        ICommand internalCommand
    )
    {
        AddressPool = addressPool;
        FrontendIp = frontendIp;
        InboundNatPool = inboundNatPool;
        InboundNatRule = inboundNatRule;
        OutboundRule = outboundRule;
        Probe = probe;
        Rule = rule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkLbAddressPool AddressPool { get; }

    public AzNetworkLbFrontendIp FrontendIp { get; }

    public AzNetworkLbInboundNatPool InboundNatPool { get; }

    public AzNetworkLbInboundNatRule InboundNatRule { get; }

    public AzNetworkLbOutboundRule OutboundRule { get; }

    public AzNetworkLbProbe Probe { get; }

    public AzNetworkLbRule Rule { get; }

    public async Task<CommandResult> Create(AzNetworkLbCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkLbDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkLbListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMapping(AzNetworkLbListMappingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListNic(AzNetworkLbListNicOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkLbShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkLbShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkLbUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkLbUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkLbWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkLbWaitOptions(), token);
    }
}