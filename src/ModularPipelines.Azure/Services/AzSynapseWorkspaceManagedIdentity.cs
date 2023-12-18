using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "workspace")]
public class AzSynapseWorkspaceManagedIdentity
{
    public AzSynapseWorkspaceManagedIdentity(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> GrantSqlAccess(AzSynapseWorkspaceManagedIdentityGrantSqlAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseWorkspaceManagedIdentityGrantSqlAccessOptions(), token);
    }

    public async Task<CommandResult> RevokeSqlAccess(AzSynapseWorkspaceManagedIdentityRevokeSqlAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseWorkspaceManagedIdentityRevokeSqlAccessOptions(), token);
    }

    public async Task<CommandResult> ShowSqlAccess(AzSynapseWorkspaceManagedIdentityShowSqlAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseWorkspaceManagedIdentityShowSqlAccessOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSynapseWorkspaceManagedIdentityWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseWorkspaceManagedIdentityWaitOptions(), token);
    }
}