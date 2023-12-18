using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> Create(AzPolicyAssignmentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPolicyAssignmentDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPolicyAssignmentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
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

