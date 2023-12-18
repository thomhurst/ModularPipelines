using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "policy", "rule-collection-group")]
public class AzNetworkFirewallPolicyRuleCollectionGroupCollection
{
    public AzNetworkFirewallPolicyRuleCollectionGroupCollection(
        AzNetworkFirewallPolicyRuleCollectionGroupCollectionRule rule,
        ICommand internalCommand
    )
    {
        Rule = rule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFirewallPolicyRuleCollectionGroupCollectionRule Rule { get; }

    public async Task<CommandResult> AddFilterCollection(AzNetworkFirewallPolicyRuleCollectionGroupCollectionAddFilterCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddNatCollection(AzNetworkFirewallPolicyRuleCollectionGroupCollectionAddNatCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkFirewallPolicyRuleCollectionGroupCollectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzNetworkFirewallPolicyRuleCollectionGroupCollectionRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}