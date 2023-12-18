using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall")]
public class AzNetworkFirewallApplicationRule
{
    public AzNetworkFirewallApplicationRule(
        AzNetworkFirewallApplicationRuleCollection collection,
        ICommand internalCommand
    )
    {
        Collection = collection;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFirewallApplicationRuleCollection Collection { get; }

    public async Task<CommandResult> Create(AzNetworkFirewallApplicationRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFirewallApplicationRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallApplicationRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkFirewallApplicationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkFirewallApplicationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallApplicationRuleShowOptions(), token);
    }
}