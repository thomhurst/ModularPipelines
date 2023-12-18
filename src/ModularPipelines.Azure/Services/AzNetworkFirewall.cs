using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkFirewall
{
    public AzNetworkFirewall(
        AzNetworkFirewallApplicationRule applicationRule,
        AzNetworkFirewallIpConfig ipConfig,
        AzNetworkFirewallManagementIpConfig managementIpConfig,
        AzNetworkFirewallNatRule natRule,
        AzNetworkFirewallNetworkRule networkRule,
        AzNetworkFirewallPolicy policy,
        AzNetworkFirewallThreatIntelAllowlist threatIntelAllowlist,
        ICommand internalCommand
    )
    {
        ApplicationRule = applicationRule;
        IpConfig = ipConfig;
        ManagementIpConfig = managementIpConfig;
        NatRule = natRule;
        NetworkRule = networkRule;
        Policy = policy;
        ThreatIntelAllowlist = threatIntelAllowlist;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkFirewallApplicationRule ApplicationRule { get; }

    public AzNetworkFirewallIpConfig IpConfig { get; }

    public AzNetworkFirewallManagementIpConfig ManagementIpConfig { get; }

    public AzNetworkFirewallNatRule NatRule { get; }

    public AzNetworkFirewallNetworkRule NetworkRule { get; }

    public AzNetworkFirewallPolicy Policy { get; }

    public AzNetworkFirewallThreatIntelAllowlist ThreatIntelAllowlist { get; }

    public async Task<CommandResult> Create(AzNetworkFirewallCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFirewallDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallDeleteOptions(), token);
    }

    public async Task<CommandResult> LearnedIpPrefix(AzNetworkFirewallLearnedIpPrefixOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallLearnedIpPrefixOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkFirewallListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallListOptions(), token);
    }

    public async Task<CommandResult> ListFqdnTags(AzNetworkFirewallListFqdnTagsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallListFqdnTagsOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkFirewallShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkFirewallUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkFirewallWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFirewallWaitOptions(), token);
    }
}