using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto")]
public class AzKustoDatabasePrincipalAssignment
{
    public AzKustoDatabasePrincipalAssignment(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzKustoDatabasePrincipalAssignmentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzKustoDatabasePrincipalAssignmentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDatabasePrincipalAssignmentDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzKustoDatabasePrincipalAssignmentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzKustoDatabasePrincipalAssignmentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDatabasePrincipalAssignmentShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzKustoDatabasePrincipalAssignmentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDatabasePrincipalAssignmentUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzKustoDatabasePrincipalAssignmentWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDatabasePrincipalAssignmentWaitOptions(), cancellationToken: token);
    }
}