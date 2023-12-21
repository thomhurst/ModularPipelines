using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "network-rule")]
public class AzNetworkFirewallNetworkRuleCollection
{
    public AzNetworkFirewallNetworkRuleCollection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzNetworkFirewallNetworkRuleCollectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallNetworkRuleCollectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkFirewallNetworkRuleCollectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkFirewallNetworkRuleCollectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallNetworkRuleCollectionShowOptions(), token);
    }
}