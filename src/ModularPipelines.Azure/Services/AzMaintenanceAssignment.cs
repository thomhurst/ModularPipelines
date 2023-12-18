using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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

    public async Task<CommandResult> CreateOrUpdateParent(AzMaintenanceAssignmentCreateOrUpdateParentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentCreateOrUpdateParentOptions(), token);
    }

    public async Task<CommandResult> CreateOrUpdateResourceGroup(AzMaintenanceAssignmentCreateOrUpdateResourceGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentCreateOrUpdateResourceGroupOptions(), token);
    }

    public async Task<CommandResult> CreateOrUpdateSubscription(AzMaintenanceAssignmentCreateOrUpdateSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentCreateOrUpdateSubscriptionOptions(), token);
    }

    public async Task<CommandResult> Delete(AzMaintenanceAssignmentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentDeleteOptions(), token);
    }

    public async Task<CommandResult> DeleteParent(AzMaintenanceAssignmentDeleteParentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentDeleteParentOptions(), token);
    }

    public async Task<CommandResult> DeleteResourceGroup(AzMaintenanceAssignmentDeleteResourceGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentDeleteResourceGroupOptions(), token);
    }

    public async Task<CommandResult> DeleteSubscription(AzMaintenanceAssignmentDeleteSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceAssignmentDeleteSubscriptionOptions(), token);
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