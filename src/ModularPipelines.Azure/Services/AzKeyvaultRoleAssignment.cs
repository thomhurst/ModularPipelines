using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "role")]
public class AzKeyvaultRoleAssignment
{
    public AzKeyvaultRoleAssignment(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzKeyvaultRoleAssignmentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzKeyvaultRoleAssignmentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultRoleAssignmentDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzKeyvaultRoleAssignmentListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultRoleAssignmentListOptions(), token);
    }
}