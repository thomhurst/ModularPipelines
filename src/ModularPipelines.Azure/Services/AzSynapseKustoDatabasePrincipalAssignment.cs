using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "kusto")]
public class AzSynapseKustoDatabasePrincipalAssignment
{
    public AzSynapseKustoDatabasePrincipalAssignment(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzSynapseKustoDatabasePrincipalAssignmentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSynapseKustoDatabasePrincipalAssignmentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoDatabasePrincipalAssignmentDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSynapseKustoDatabasePrincipalAssignmentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSynapseKustoDatabasePrincipalAssignmentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoDatabasePrincipalAssignmentShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSynapseKustoDatabasePrincipalAssignmentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoDatabasePrincipalAssignmentUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSynapseKustoDatabasePrincipalAssignmentWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoDatabasePrincipalAssignmentWaitOptions(), token);
    }
}