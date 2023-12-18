using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account")]
public class AzAccountManagementGroup
{
    public AzAccountManagementGroup(
        AzAccountManagementGroupEntities entities,
        AzAccountManagementGroupHierarchySettings hierarchySettings,
        AzAccountManagementGroupSubscription subscription,
        AzAccountManagementGroupTenantBackfill tenantBackfill,
        ICommand internalCommand
    )
    {
        Entities = entities;
        HierarchySettings = hierarchySettings;
        Subscription = subscription;
        TenantBackfill = tenantBackfill;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAccountManagementGroupEntities Entities { get; }

    public AzAccountManagementGroupHierarchySettings HierarchySettings { get; }

    public AzAccountManagementGroupSubscription Subscription { get; }

    public AzAccountManagementGroupTenantBackfill TenantBackfill { get; }

    public async Task<CommandResult> CheckNameAvailability(AzAccountManagementGroupCheckNameAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzAccountManagementGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAccountManagementGroupDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAccountManagementGroupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAccountManagementGroupShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAccountManagementGroupUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}