using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall")]
public class AzNetworkFirewallPolicy
{
    public AzNetworkFirewallPolicy(
        AzNetworkFirewallPolicyIntrusionDetection intrusionDetection,
        AzNetworkFirewallPolicyRuleCollectionGroup ruleCollectionGroup,
        ICommand internalCommand
    )
    {
        IntrusionDetection = intrusionDetection;
        RuleCollectionGroup = ruleCollectionGroup;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFirewallPolicyIntrusionDetection IntrusionDetection { get; }

    public AzNetworkFirewallPolicyRuleCollectionGroup RuleCollectionGroup { get; }

    public async Task<CommandResult> Create(AzNetworkFirewallPolicyCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFirewallPolicyDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallPolicyDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkFirewallPolicyListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallPolicyListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkFirewallPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallPolicyShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkFirewallPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallPolicyUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkFirewallPolicyWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallPolicyWaitOptions(), token);
    }
}