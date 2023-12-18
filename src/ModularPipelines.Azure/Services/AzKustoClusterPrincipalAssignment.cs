using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzKustoClusterPrincipalAssignmentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterPrincipalAssignmentDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzKustoClusterPrincipalAssignmentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzKustoClusterPrincipalAssignmentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterPrincipalAssignmentShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzKustoClusterPrincipalAssignmentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterPrincipalAssignmentUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzKustoClusterPrincipalAssignmentWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterPrincipalAssignmentWaitOptions(), token);
    }
}