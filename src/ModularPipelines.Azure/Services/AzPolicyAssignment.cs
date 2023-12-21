using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy")]
public class AzPolicyAssignment
{
    public AzPolicyAssignment(
        AzPolicyAssignmentIdentity identity,
        AzPolicyAssignmentNonComplianceMessage nonComplianceMessage,
        ICommand internalCommand
    )
    {
        Identity = identity;
        NonComplianceMessage = nonComplianceMessage;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPolicyAssignmentIdentity Identity { get; }

    public AzPolicyAssignmentNonComplianceMessage NonComplianceMessage { get; }

    public async Task<CommandResult> Create(AzPolicyAssignmentCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPolicyAssignmentCreateOptions(), token);
    }

    public async Task<CommandResult> Delete(AzPolicyAssignmentDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPolicyAssignmentListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPolicyAssignmentListOptions(), token);
    }

    public async Task<CommandResult> Show(AzPolicyAssignmentShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzPolicyAssignmentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPolicyAssignmentUpdateOptions(), token);
    }
}