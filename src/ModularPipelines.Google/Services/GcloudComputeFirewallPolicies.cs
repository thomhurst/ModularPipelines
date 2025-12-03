using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("compute")]
public class GcloudComputeFirewallPolicies
{
    public GcloudComputeFirewallPolicies(
        GcloudComputeFirewallPoliciesAssociations associations,
        GcloudComputeFirewallPoliciesRules rules,
        ICommand internalCommand
    )
    {
        Associations = associations;
        Rules = rules;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeFirewallPoliciesAssociations Associations { get; }

    public GcloudComputeFirewallPoliciesRules Rules { get; }

    public async Task<CommandResult> CloneRules(GcloudComputeFirewallPoliciesCloneRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudComputeFirewallPoliciesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudComputeFirewallPoliciesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComputeFirewallPoliciesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeFirewallPoliciesListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRules(GcloudComputeFirewallPoliciesListRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Move(GcloudComputeFirewallPoliciesMoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudComputeFirewallPoliciesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}