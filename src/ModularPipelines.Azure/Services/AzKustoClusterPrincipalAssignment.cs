using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto")]
public class AzKustoClusterPrincipalAssignment
{
    public AzKustoClusterPrincipalAssignment(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzKustoClusterPrincipalAssignmentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzKustoClusterPrincipalAssignmentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterPrincipalAssignmentDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzKustoClusterPrincipalAssignmentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzKustoClusterPrincipalAssignmentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterPrincipalAssignmentShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzKustoClusterPrincipalAssignmentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterPrincipalAssignmentUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzKustoClusterPrincipalAssignmentWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterPrincipalAssignmentWaitOptions(), cancellationToken: token);
    }
}