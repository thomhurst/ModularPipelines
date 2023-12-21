using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "application-rule")]
public class AzNetworkFirewallApplicationRuleCollection
{
    public AzNetworkFirewallApplicationRuleCollection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzNetworkFirewallApplicationRuleCollectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallApplicationRuleCollectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkFirewallApplicationRuleCollectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkFirewallApplicationRuleCollectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallApplicationRuleCollectionShowOptions(), token);
    }
}