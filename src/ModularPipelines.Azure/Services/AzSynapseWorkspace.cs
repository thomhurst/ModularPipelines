using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse")]
public class AzSynapseWorkspace
{
    public AzSynapseWorkspace(
        AzSynapseWorkspaceFirewallRule firewallRule,
        AzSynapseWorkspaceKey key,
        AzSynapseWorkspaceManagedIdentity managedIdentity,
        ICommand internalCommand
    )
    {
        FirewallRule = firewallRule;
        Key = key;
        ManagedIdentity = managedIdentity;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSynapseWorkspaceFirewallRule FirewallRule { get; }

    public AzSynapseWorkspaceKey Key { get; }

    public AzSynapseWorkspaceManagedIdentity ManagedIdentity { get; }

    public async Task<CommandResult> Activate(AzSynapseWorkspaceActivateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckName(AzSynapseWorkspaceCheckNameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzSynapseWorkspaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSynapseWorkspaceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSynapseWorkspaceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSynapseWorkspaceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSynapseWorkspaceUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzSynapseWorkspaceWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}