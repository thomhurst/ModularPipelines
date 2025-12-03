using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("compute")]
public class GcloudComputeNetworkFirewallPolicies
{
    public GcloudComputeNetworkFirewallPolicies(
        GcloudComputeNetworkFirewallPoliciesAssociations associations,
        GcloudComputeNetworkFirewallPoliciesRules rules,
        ICommand internalCommand
    )
    {
        Associations = associations;
        Rules = rules;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeNetworkFirewallPoliciesAssociations Associations { get; }

    public GcloudComputeNetworkFirewallPoliciesRules Rules { get; }

    public async Task<CommandResult> CloneRules(GcloudComputeNetworkFirewallPoliciesCloneRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudComputeNetworkFirewallPoliciesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudComputeNetworkFirewallPoliciesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComputeNetworkFirewallPoliciesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEffectiveFirewalls(GcloudComputeNetworkFirewallPoliciesGetEffectiveFirewallsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeNetworkFirewallPoliciesListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudComputeNetworkFirewallPoliciesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}