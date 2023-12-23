using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsRedshiftServerless
{
    public AwsRedshiftServerless(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ConvertRecoveryPointToSnapshot(AwsRedshiftServerlessConvertRecoveryPointToSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomDomainAssociation(AwsRedshiftServerlessCreateCustomDomainAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEndpointAccess(AwsRedshiftServerlessCreateEndpointAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNamespace(AwsRedshiftServerlessCreateNamespaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateScheduledAction(AwsRedshiftServerlessCreateScheduledActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSnapshot(AwsRedshiftServerlessCreateSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSnapshotCopyConfiguration(AwsRedshiftServerlessCreateSnapshotCopyConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUsageLimit(AwsRedshiftServerlessCreateUsageLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorkgroup(AwsRedshiftServerlessCreateWorkgroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomDomainAssociation(AwsRedshiftServerlessDeleteCustomDomainAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEndpointAccess(AwsRedshiftServerlessDeleteEndpointAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNamespace(AwsRedshiftServerlessDeleteNamespaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsRedshiftServerlessDeleteResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteScheduledAction(AwsRedshiftServerlessDeleteScheduledActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSnapshot(AwsRedshiftServerlessDeleteSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSnapshotCopyConfiguration(AwsRedshiftServerlessDeleteSnapshotCopyConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUsageLimit(AwsRedshiftServerlessDeleteUsageLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWorkgroup(AwsRedshiftServerlessDeleteWorkgroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCredentials(AwsRedshiftServerlessGetCredentialsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessGetCredentialsOptions(), token);
    }

    public async Task<CommandResult> GetCustomDomainAssociation(AwsRedshiftServerlessGetCustomDomainAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEndpointAccess(AwsRedshiftServerlessGetEndpointAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNamespace(AwsRedshiftServerlessGetNamespaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRecoveryPoint(AwsRedshiftServerlessGetRecoveryPointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourcePolicy(AwsRedshiftServerlessGetResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetScheduledAction(AwsRedshiftServerlessGetScheduledActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSnapshot(AwsRedshiftServerlessGetSnapshotOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessGetSnapshotOptions(), token);
    }

    public async Task<CommandResult> GetTableRestoreStatus(AwsRedshiftServerlessGetTableRestoreStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUsageLimit(AwsRedshiftServerlessGetUsageLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWorkgroup(AwsRedshiftServerlessGetWorkgroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCustomDomainAssociations(AwsRedshiftServerlessListCustomDomainAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessListCustomDomainAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListEndpointAccess(AwsRedshiftServerlessListEndpointAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessListEndpointAccessOptions(), token);
    }

    public async Task<CommandResult> ListNamespaces(AwsRedshiftServerlessListNamespacesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessListNamespacesOptions(), token);
    }

    public async Task<CommandResult> ListRecoveryPoints(AwsRedshiftServerlessListRecoveryPointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessListRecoveryPointsOptions(), token);
    }

    public async Task<CommandResult> ListScheduledActions(AwsRedshiftServerlessListScheduledActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessListScheduledActionsOptions(), token);
    }

    public async Task<CommandResult> ListSnapshotCopyConfigurations(AwsRedshiftServerlessListSnapshotCopyConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessListSnapshotCopyConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListSnapshots(AwsRedshiftServerlessListSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessListSnapshotsOptions(), token);
    }

    public async Task<CommandResult> ListTableRestoreStatus(AwsRedshiftServerlessListTableRestoreStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessListTableRestoreStatusOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsRedshiftServerlessListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUsageLimits(AwsRedshiftServerlessListUsageLimitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessListUsageLimitsOptions(), token);
    }

    public async Task<CommandResult> ListWorkgroups(AwsRedshiftServerlessListWorkgroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftServerlessListWorkgroupsOptions(), token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsRedshiftServerlessPutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreFromRecoveryPoint(AwsRedshiftServerlessRestoreFromRecoveryPointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreFromSnapshot(AwsRedshiftServerlessRestoreFromSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreTableFromRecoveryPoint(AwsRedshiftServerlessRestoreTableFromRecoveryPointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreTableFromSnapshot(AwsRedshiftServerlessRestoreTableFromSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsRedshiftServerlessTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsRedshiftServerlessUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCustomDomainAssociation(AwsRedshiftServerlessUpdateCustomDomainAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEndpointAccess(AwsRedshiftServerlessUpdateEndpointAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateNamespace(AwsRedshiftServerlessUpdateNamespaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateScheduledAction(AwsRedshiftServerlessUpdateScheduledActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSnapshot(AwsRedshiftServerlessUpdateSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSnapshotCopyConfiguration(AwsRedshiftServerlessUpdateSnapshotCopyConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateUsageLimit(AwsRedshiftServerlessUpdateUsageLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWorkgroup(AwsRedshiftServerlessUpdateWorkgroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}