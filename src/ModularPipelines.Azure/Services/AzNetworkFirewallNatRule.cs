using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall")]
public class AzNetworkFirewallNatRule
{
    public AzNetworkFirewallNatRule(
        AzNetworkFirewallNatRuleCollection collection,
        ICommand internalCommand
    )
    {
        Collection = collection;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFirewallNatRuleCollection Collection { get; }

    public async Task<CommandResult> Create(AzNetworkFirewallNatRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFirewallNatRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallNatRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkFirewallNatRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkFirewallNatRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallNatRuleShowOptions(), token);
    }
}