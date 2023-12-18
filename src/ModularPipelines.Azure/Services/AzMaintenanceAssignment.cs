using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maintenance")]
public class AzMaintenanceAssignment
{
    public AzMaintenanceAssignment(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMaintenanceAssignmentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOrUpdateParent(AzMaintenanceAssignmentCreateOrUpdateParentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOrUpdateResourceGroup(AzMaintenanceAssignmentCreateOrUpdateResourceGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOrUpdateSubscription(AzMaintenanceAssignmentCreateOrUpdateSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMaintenanceAssignmentDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteParent(AzMaintenanceAssignmentDeleteParentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourceGroup(AzMaintenanceAssignmentDeleteResourceGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSubscription(AzMaintenanceAssignmentDeleteSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMaintenanceAssignmentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListParent(AzMaintenanceAssignmentListParentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentListParentOptions(), token);
    }

    public async Task<CommandResult> ListSubscription(AzMaintenanceAssignmentListSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentListSubscriptionOptions(), token);
    }

    public async Task<CommandResult> Show(AzMaintenanceAssignmentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentShowOptions(), token);
    }

    public async Task<CommandResult> ShowParent(AzMaintenanceAssignmentShowParentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentShowParentOptions(), token);
    }

    public async Task<CommandResult> ShowResourceGroup(AzMaintenanceAssignmentShowResourceGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentShowResourceGroupOptions(), token);
    }

    public async Task<CommandResult> ShowSubscription(AzMaintenanceAssignmentShowSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentShowSubscriptionOptions(), token);
    }

    public async Task<CommandResult> Update(AzMaintenanceAssignmentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentUpdateOptions(), token);
    }

    public async Task<CommandResult> UpdateParent(AzMaintenanceAssignmentUpdateParentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentUpdateParentOptions(), token);
    }

    public async Task<CommandResult> UpdateResourceGroup(AzMaintenanceAssignmentUpdateResourceGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentUpdateResourceGroupOptions(), token);
    }

    public async Task<CommandResult> UpdateSubscription(AzMaintenanceAssignmentUpdateSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentUpdateSubscriptionOptions(), token);
    }
}