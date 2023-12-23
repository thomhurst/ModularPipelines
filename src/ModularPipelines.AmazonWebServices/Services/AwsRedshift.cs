using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsRedshift
{
    public AwsRedshift(
        AwsRedshiftWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsRedshiftWait Wait { get; }

    public async Task<CommandResult> AcceptReservedNodeExchange(AwsRedshiftAcceptReservedNodeExchangeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddPartner(AwsRedshiftAddPartnerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateDataShareConsumer(AwsRedshiftAssociateDataShareConsumerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthorizeClusterSecurityGroupIngress(AwsRedshiftAuthorizeClusterSecurityGroupIngressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthorizeDataShare(AwsRedshiftAuthorizeDataShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthorizeEndpointAccess(AwsRedshiftAuthorizeEndpointAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthorizeSnapshotAccess(AwsRedshiftAuthorizeSnapshotAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeleteClusterSnapshots(AwsRedshiftBatchDeleteClusterSnapshotsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchModifyClusterSnapshots(AwsRedshiftBatchModifyClusterSnapshotsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelResize(AwsRedshiftCancelResizeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyClusterSnapshot(AwsRedshiftCopyClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAuthenticationProfile(AwsRedshiftCreateAuthenticationProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCluster(AwsRedshiftCreateClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateClusterParameterGroup(AwsRedshiftCreateClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateClusterSecurityGroup(AwsRedshiftCreateClusterSecurityGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateClusterSnapshot(AwsRedshiftCreateClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateClusterSubnetGroup(AwsRedshiftCreateClusterSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomDomainAssociation(AwsRedshiftCreateCustomDomainAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEndpointAccess(AwsRedshiftCreateEndpointAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEventSubscription(AwsRedshiftCreateEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateHsmClientCertificate(AwsRedshiftCreateHsmClientCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateHsmConfiguration(AwsRedshiftCreateHsmConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRedshiftIdcApplication(AwsRedshiftCreateRedshiftIdcApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateScheduledAction(AwsRedshiftCreateScheduledActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSnapshotCopyGrant(AwsRedshiftCreateSnapshotCopyGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSnapshotSchedule(AwsRedshiftCreateSnapshotScheduleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftCreateSnapshotScheduleOptions(), token);
    }

    public async Task<CommandResult> CreateTags(AwsRedshiftCreateTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUsageLimit(AwsRedshiftCreateUsageLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeauthorizeDataShare(AwsRedshiftDeauthorizeDataShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAuthenticationProfile(AwsRedshiftDeleteAuthenticationProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCluster(AwsRedshiftDeleteClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteClusterParameterGroup(AwsRedshiftDeleteClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteClusterSecurityGroup(AwsRedshiftDeleteClusterSecurityGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteClusterSnapshot(AwsRedshiftDeleteClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteClusterSubnetGroup(AwsRedshiftDeleteClusterSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomDomainAssociation(AwsRedshiftDeleteCustomDomainAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEndpointAccess(AwsRedshiftDeleteEndpointAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEventSubscription(AwsRedshiftDeleteEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteHsmClientCertificate(AwsRedshiftDeleteHsmClientCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteHsmConfiguration(AwsRedshiftDeleteHsmConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePartner(AwsRedshiftDeletePartnerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRedshiftIdcApplication(AwsRedshiftDeleteRedshiftIdcApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsRedshiftDeleteResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteScheduledAction(AwsRedshiftDeleteScheduledActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSnapshotCopyGrant(AwsRedshiftDeleteSnapshotCopyGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSnapshotSchedule(AwsRedshiftDeleteSnapshotScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTags(AwsRedshiftDeleteTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUsageLimit(AwsRedshiftDeleteUsageLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountAttributes(AwsRedshiftDescribeAccountAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeAccountAttributesOptions(), token);
    }

    public async Task<CommandResult> DescribeAuthenticationProfiles(AwsRedshiftDescribeAuthenticationProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeAuthenticationProfilesOptions(), token);
    }

    public async Task<CommandResult> DescribeClusterDbRevisions(AwsRedshiftDescribeClusterDbRevisionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterDbRevisionsOptions(), token);
    }

    public async Task<CommandResult> DescribeClusterParameterGroups(AwsRedshiftDescribeClusterParameterGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterParameterGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeClusterParameters(AwsRedshiftDescribeClusterParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeClusterSecurityGroups(AwsRedshiftDescribeClusterSecurityGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterSecurityGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeClusterSnapshots(AwsRedshiftDescribeClusterSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterSnapshotsOptions(), token);
    }

    public async Task<CommandResult> DescribeClusterSubnetGroups(AwsRedshiftDescribeClusterSubnetGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterSubnetGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeClusterTracks(AwsRedshiftDescribeClusterTracksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterTracksOptions(), token);
    }

    public async Task<CommandResult> DescribeClusterVersions(AwsRedshiftDescribeClusterVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterVersionsOptions(), token);
    }

    public async Task<CommandResult> DescribeClusters(AwsRedshiftDescribeClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClustersOptions(), token);
    }

    public async Task<CommandResult> DescribeCustomDomainAssociations(AwsRedshiftDescribeCustomDomainAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeCustomDomainAssociationsOptions(), token);
    }

    public async Task<CommandResult> DescribeDataShares(AwsRedshiftDescribeDataSharesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeDataSharesOptions(), token);
    }

    public async Task<CommandResult> DescribeDataSharesForConsumer(AwsRedshiftDescribeDataSharesForConsumerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeDataSharesForConsumerOptions(), token);
    }

    public async Task<CommandResult> DescribeDataSharesForProducer(AwsRedshiftDescribeDataSharesForProducerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeDataSharesForProducerOptions(), token);
    }

    public async Task<CommandResult> DescribeDefaultClusterParameters(AwsRedshiftDescribeDefaultClusterParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEndpointAccess(AwsRedshiftDescribeEndpointAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeEndpointAccessOptions(), token);
    }

    public async Task<CommandResult> DescribeEndpointAuthorization(AwsRedshiftDescribeEndpointAuthorizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeEndpointAuthorizationOptions(), token);
    }

    public async Task<CommandResult> DescribeEventCategories(AwsRedshiftDescribeEventCategoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeEventCategoriesOptions(), token);
    }

    public async Task<CommandResult> DescribeEventSubscriptions(AwsRedshiftDescribeEventSubscriptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeEventSubscriptionsOptions(), token);
    }

    public async Task<CommandResult> DescribeEvents(AwsRedshiftDescribeEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeEventsOptions(), token);
    }

    public async Task<CommandResult> DescribeHsmClientCertificates(AwsRedshiftDescribeHsmClientCertificatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeHsmClientCertificatesOptions(), token);
    }

    public async Task<CommandResult> DescribeHsmConfigurations(AwsRedshiftDescribeHsmConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeHsmConfigurationsOptions(), token);
    }

    public async Task<CommandResult> DescribeInboundIntegrations(AwsRedshiftDescribeInboundIntegrationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeInboundIntegrationsOptions(), token);
    }

    public async Task<CommandResult> DescribeLoggingStatus(AwsRedshiftDescribeLoggingStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeNodeConfigurationOptions(AwsRedshiftDescribeNodeConfigurationOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOrderableClusterOptions(AwsRedshiftDescribeOrderableClusterOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeOrderableClusterOptionsOptions(), token);
    }

    public async Task<CommandResult> DescribePartners(AwsRedshiftDescribePartnersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRedshiftIdcApplications(AwsRedshiftDescribeRedshiftIdcApplicationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeRedshiftIdcApplicationsOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedNodeExchangeStatus(AwsRedshiftDescribeReservedNodeExchangeStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeReservedNodeExchangeStatusOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedNodeOfferings(AwsRedshiftDescribeReservedNodeOfferingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeReservedNodeOfferingsOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedNodes(AwsRedshiftDescribeReservedNodesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeReservedNodesOptions(), token);
    }

    public async Task<CommandResult> DescribeResize(AwsRedshiftDescribeResizeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeScheduledActions(AwsRedshiftDescribeScheduledActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeScheduledActionsOptions(), token);
    }

    public async Task<CommandResult> DescribeSnapshotCopyGrants(AwsRedshiftDescribeSnapshotCopyGrantsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeSnapshotCopyGrantsOptions(), token);
    }

    public async Task<CommandResult> DescribeSnapshotSchedules(AwsRedshiftDescribeSnapshotSchedulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeSnapshotSchedulesOptions(), token);
    }

    public async Task<CommandResult> DescribeStorage(AwsRedshiftDescribeStorageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeStorageOptions(), token);
    }

    public async Task<CommandResult> DescribeTableRestoreStatus(AwsRedshiftDescribeTableRestoreStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeTableRestoreStatusOptions(), token);
    }

    public async Task<CommandResult> DescribeTags(AwsRedshiftDescribeTagsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeTagsOptions(), token);
    }

    public async Task<CommandResult> DescribeUsageLimits(AwsRedshiftDescribeUsageLimitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeUsageLimitsOptions(), token);
    }

    public async Task<CommandResult> DisableLogging(AwsRedshiftDisableLoggingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableSnapshotCopy(AwsRedshiftDisableSnapshotCopyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateDataShareConsumer(AwsRedshiftDisassociateDataShareConsumerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableLogging(AwsRedshiftEnableLoggingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableSnapshotCopy(AwsRedshiftEnableSnapshotCopyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FailoverPrimaryCompute(AwsRedshiftFailoverPrimaryComputeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetClusterCredentials(AwsRedshiftGetClusterCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetClusterCredentialsWithIam(AwsRedshiftGetClusterCredentialsWithIamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftGetClusterCredentialsWithIamOptions(), token);
    }

    public async Task<CommandResult> GetReservedNodeExchangeConfigurationOptions(AwsRedshiftGetReservedNodeExchangeConfigurationOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReservedNodeExchangeOfferings(AwsRedshiftGetReservedNodeExchangeOfferingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourcePolicy(AwsRedshiftGetResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyAquaConfiguration(AwsRedshiftModifyAquaConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyAuthenticationProfile(AwsRedshiftModifyAuthenticationProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyCluster(AwsRedshiftModifyClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyClusterDbRevision(AwsRedshiftModifyClusterDbRevisionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyClusterIamRoles(AwsRedshiftModifyClusterIamRolesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyClusterMaintenance(AwsRedshiftModifyClusterMaintenanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyClusterParameterGroup(AwsRedshiftModifyClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyClusterSnapshot(AwsRedshiftModifyClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyClusterSnapshotSchedule(AwsRedshiftModifyClusterSnapshotScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyClusterSubnetGroup(AwsRedshiftModifyClusterSubnetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyCustomDomainAssociation(AwsRedshiftModifyCustomDomainAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyEndpointAccess(AwsRedshiftModifyEndpointAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyEventSubscription(AwsRedshiftModifyEventSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyRedshiftIdcApplication(AwsRedshiftModifyRedshiftIdcApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyScheduledAction(AwsRedshiftModifyScheduledActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifySnapshotCopyRetentionPeriod(AwsRedshiftModifySnapshotCopyRetentionPeriodOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifySnapshotSchedule(AwsRedshiftModifySnapshotScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyUsageLimit(AwsRedshiftModifyUsageLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PauseCluster(AwsRedshiftPauseClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PurchaseReservedNodeOffering(AwsRedshiftPurchaseReservedNodeOfferingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsRedshiftPutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootCluster(AwsRedshiftRebootClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectDataShare(AwsRedshiftRejectDataShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetClusterParameterGroup(AwsRedshiftResetClusterParameterGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResizeCluster(AwsRedshiftResizeClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreFromClusterSnapshot(AwsRedshiftRestoreFromClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreTableFromClusterSnapshot(AwsRedshiftRestoreTableFromClusterSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResumeCluster(AwsRedshiftResumeClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokeClusterSecurityGroupIngress(AwsRedshiftRevokeClusterSecurityGroupIngressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokeEndpointAccess(AwsRedshiftRevokeEndpointAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftRevokeEndpointAccessOptions(), token);
    }

    public async Task<CommandResult> RevokeSnapshotAccess(AwsRedshiftRevokeSnapshotAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RotateEncryptionKey(AwsRedshiftRotateEncryptionKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePartnerStatus(AwsRedshiftUpdatePartnerStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}