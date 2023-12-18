using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "policy")]
public class AzNetworkFirewallPolicyRuleCollectionGroup
{
    public AzNetworkFirewallPolicyRuleCollectionGroup(
        AzNetworkFirewallPolicyRuleCollectionGroupCollection collection,
        ICommand internalCommand
    )
    {
        Collection = collection;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFirewallPolicyRuleCollectionGroupCollection Collection { get; }

    public async Task<CommandResult> Create(AzNetworkFirewallPolicyRuleCollectionGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFirewallPolicyRuleCollectionGroupDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkFirewallPolicyRuleCollectionGroupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkFirewallPolicyRuleCollectionGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallPolicyRuleCollectionGroupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkFirewallPolicyRuleCollectionGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallPolicyRuleCollectionGroupUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkFirewallPolicyRuleCollectionGroupWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallPolicyRuleCollectionGroupWaitOptions(), token);
    }
}