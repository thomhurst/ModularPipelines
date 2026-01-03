using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "workspace")]
public class AzSynapseWorkspaceFirewallRule
{
    public AzSynapseWorkspaceFirewallRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzSynapseWorkspaceFirewallRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzSynapseWorkspaceFirewallRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseWorkspaceFirewallRuleDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzSynapseWorkspaceFirewallRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzSynapseWorkspaceFirewallRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseWorkspaceFirewallRuleShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzSynapseWorkspaceFirewallRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseWorkspaceFirewallRuleUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzSynapseWorkspaceFirewallRuleWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }
}