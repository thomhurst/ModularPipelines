using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> AcceptReservedNodeExchange(AwsRedshiftAcceptReservedNodeExchangeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AddPartner(AwsRedshiftAddPartnerOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateDataShareConsumer(AwsRedshiftAssociateDataShareConsumerOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AuthorizeClusterSecurityGroupIngress(AwsRedshiftAuthorizeClusterSecurityGroupIngressOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AuthorizeDataShare(AwsRedshiftAuthorizeDataShareOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AuthorizeEndpointAccess(AwsRedshiftAuthorizeEndpointAccessOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AuthorizeSnapshotAccess(AwsRedshiftAuthorizeSnapshotAccessOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchDeleteClusterSnapshots(AwsRedshiftBatchDeleteClusterSnapshotsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchModifyClusterSnapshots(AwsRedshiftBatchModifyClusterSnapshotsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelResize(AwsRedshiftCancelResizeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CopyClusterSnapshot(AwsRedshiftCopyClusterSnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateAuthenticationProfile(AwsRedshiftCreateAuthenticationProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateCluster(AwsRedshiftCreateClusterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateClusterParameterGroup(AwsRedshiftCreateClusterParameterGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateClusterSecurityGroup(AwsRedshiftCreateClusterSecurityGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateClusterSnapshot(AwsRedshiftCreateClusterSnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateClusterSubnetGroup(AwsRedshiftCreateClusterSubnetGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateCustomDomainAssociation(AwsRedshiftCreateCustomDomainAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateEndpointAccess(AwsRedshiftCreateEndpointAccessOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateEventSubscription(AwsRedshiftCreateEventSubscriptionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateHsmClientCertificate(AwsRedshiftCreateHsmClientCertificateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateHsmConfiguration(AwsRedshiftCreateHsmConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateRedshiftIdcApplication(AwsRedshiftCreateRedshiftIdcApplicationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateScheduledAction(AwsRedshiftCreateScheduledActionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSnapshotCopyGrant(AwsRedshiftCreateSnapshotCopyGrantOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSnapshotSchedule(AwsRedshiftCreateSnapshotScheduleOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftCreateSnapshotScheduleOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTags(AwsRedshiftCreateTagsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateUsageLimit(AwsRedshiftCreateUsageLimitOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeauthorizeDataShare(AwsRedshiftDeauthorizeDataShareOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteAuthenticationProfile(AwsRedshiftDeleteAuthenticationProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteCluster(AwsRedshiftDeleteClusterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteClusterParameterGroup(AwsRedshiftDeleteClusterParameterGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteClusterSecurityGroup(AwsRedshiftDeleteClusterSecurityGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteClusterSnapshot(AwsRedshiftDeleteClusterSnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteClusterSubnetGroup(AwsRedshiftDeleteClusterSubnetGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteCustomDomainAssociation(AwsRedshiftDeleteCustomDomainAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteEndpointAccess(AwsRedshiftDeleteEndpointAccessOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteEventSubscription(AwsRedshiftDeleteEventSubscriptionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteHsmClientCertificate(AwsRedshiftDeleteHsmClientCertificateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteHsmConfiguration(AwsRedshiftDeleteHsmConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeletePartner(AwsRedshiftDeletePartnerOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteRedshiftIdcApplication(AwsRedshiftDeleteRedshiftIdcApplicationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsRedshiftDeleteResourcePolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteScheduledAction(AwsRedshiftDeleteScheduledActionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSnapshotCopyGrant(AwsRedshiftDeleteSnapshotCopyGrantOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSnapshotSchedule(AwsRedshiftDeleteSnapshotScheduleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTags(AwsRedshiftDeleteTagsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteUsageLimit(AwsRedshiftDeleteUsageLimitOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAccountAttributes(AwsRedshiftDescribeAccountAttributesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeAccountAttributesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAuthenticationProfiles(AwsRedshiftDescribeAuthenticationProfilesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeAuthenticationProfilesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClusterDbRevisions(AwsRedshiftDescribeClusterDbRevisionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterDbRevisionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClusterParameterGroups(AwsRedshiftDescribeClusterParameterGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterParameterGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClusterParameters(AwsRedshiftDescribeClusterParametersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClusterSecurityGroups(AwsRedshiftDescribeClusterSecurityGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterSecurityGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClusterSnapshots(AwsRedshiftDescribeClusterSnapshotsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterSnapshotsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClusterSubnetGroups(AwsRedshiftDescribeClusterSubnetGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterSubnetGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClusterTracks(AwsRedshiftDescribeClusterTracksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterTracksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClusterVersions(AwsRedshiftDescribeClusterVersionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClusterVersionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeClusters(AwsRedshiftDescribeClustersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeClustersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeCustomDomainAssociations(AwsRedshiftDescribeCustomDomainAssociationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeCustomDomainAssociationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDataShares(AwsRedshiftDescribeDataSharesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeDataSharesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDataSharesForConsumer(AwsRedshiftDescribeDataSharesForConsumerOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeDataSharesForConsumerOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDataSharesForProducer(AwsRedshiftDescribeDataSharesForProducerOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeDataSharesForProducerOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDefaultClusterParameters(AwsRedshiftDescribeDefaultClusterParametersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEndpointAccess(AwsRedshiftDescribeEndpointAccessOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeEndpointAccessOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEndpointAuthorization(AwsRedshiftDescribeEndpointAuthorizationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeEndpointAuthorizationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEventCategories(AwsRedshiftDescribeEventCategoriesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeEventCategoriesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEventSubscriptions(AwsRedshiftDescribeEventSubscriptionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeEventSubscriptionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEvents(AwsRedshiftDescribeEventsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeEventsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeHsmClientCertificates(AwsRedshiftDescribeHsmClientCertificatesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeHsmClientCertificatesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeHsmConfigurations(AwsRedshiftDescribeHsmConfigurationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeHsmConfigurationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInboundIntegrations(AwsRedshiftDescribeInboundIntegrationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeInboundIntegrationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLoggingStatus(AwsRedshiftDescribeLoggingStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeNodeConfigurationOptions(AwsRedshiftDescribeNodeConfigurationOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOrderableClusterOptions(AwsRedshiftDescribeOrderableClusterOptionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeOrderableClusterOptionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePartners(AwsRedshiftDescribePartnersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRedshiftIdcApplications(AwsRedshiftDescribeRedshiftIdcApplicationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeRedshiftIdcApplicationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeReservedNodeExchangeStatus(AwsRedshiftDescribeReservedNodeExchangeStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeReservedNodeExchangeStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeReservedNodeOfferings(AwsRedshiftDescribeReservedNodeOfferingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeReservedNodeOfferingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeReservedNodes(AwsRedshiftDescribeReservedNodesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeReservedNodesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeResize(AwsRedshiftDescribeResizeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeScheduledActions(AwsRedshiftDescribeScheduledActionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeScheduledActionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSnapshotCopyGrants(AwsRedshiftDescribeSnapshotCopyGrantsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeSnapshotCopyGrantsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSnapshotSchedules(AwsRedshiftDescribeSnapshotSchedulesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeSnapshotSchedulesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeStorage(AwsRedshiftDescribeStorageOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeStorageOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTableRestoreStatus(AwsRedshiftDescribeTableRestoreStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeTableRestoreStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTags(AwsRedshiftDescribeTagsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeTagsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeUsageLimits(AwsRedshiftDescribeUsageLimitsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftDescribeUsageLimitsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableLogging(AwsRedshiftDisableLoggingOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableSnapshotCopy(AwsRedshiftDisableSnapshotCopyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateDataShareConsumer(AwsRedshiftDisassociateDataShareConsumerOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableLogging(AwsRedshiftEnableLoggingOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableSnapshotCopy(AwsRedshiftEnableSnapshotCopyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> FailoverPrimaryCompute(AwsRedshiftFailoverPrimaryComputeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetClusterCredentials(AwsRedshiftGetClusterCredentialsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetClusterCredentialsWithIam(AwsRedshiftGetClusterCredentialsWithIamOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftGetClusterCredentialsWithIamOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetReservedNodeExchangeConfigurationOptions(AwsRedshiftGetReservedNodeExchangeConfigurationOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetReservedNodeExchangeOfferings(AwsRedshiftGetReservedNodeExchangeOfferingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetResourcePolicy(AwsRedshiftGetResourcePolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyAquaConfiguration(AwsRedshiftModifyAquaConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyAuthenticationProfile(AwsRedshiftModifyAuthenticationProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyCluster(AwsRedshiftModifyClusterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyClusterDbRevision(AwsRedshiftModifyClusterDbRevisionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyClusterIamRoles(AwsRedshiftModifyClusterIamRolesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyClusterMaintenance(AwsRedshiftModifyClusterMaintenanceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyClusterParameterGroup(AwsRedshiftModifyClusterParameterGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyClusterSnapshot(AwsRedshiftModifyClusterSnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyClusterSnapshotSchedule(AwsRedshiftModifyClusterSnapshotScheduleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyClusterSubnetGroup(AwsRedshiftModifyClusterSubnetGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyCustomDomainAssociation(AwsRedshiftModifyCustomDomainAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyEndpointAccess(AwsRedshiftModifyEndpointAccessOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyEventSubscription(AwsRedshiftModifyEventSubscriptionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyRedshiftIdcApplication(AwsRedshiftModifyRedshiftIdcApplicationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyScheduledAction(AwsRedshiftModifyScheduledActionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifySnapshotCopyRetentionPeriod(AwsRedshiftModifySnapshotCopyRetentionPeriodOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifySnapshotSchedule(AwsRedshiftModifySnapshotScheduleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ModifyUsageLimit(AwsRedshiftModifyUsageLimitOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PauseCluster(AwsRedshiftPauseClusterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PurchaseReservedNodeOffering(AwsRedshiftPurchaseReservedNodeOfferingOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsRedshiftPutResourcePolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RebootCluster(AwsRedshiftRebootClusterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RejectDataShare(AwsRedshiftRejectDataShareOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResetClusterParameterGroup(AwsRedshiftResetClusterParameterGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResizeCluster(AwsRedshiftResizeClusterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RestoreFromClusterSnapshot(AwsRedshiftRestoreFromClusterSnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RestoreTableFromClusterSnapshot(AwsRedshiftRestoreTableFromClusterSnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResumeCluster(AwsRedshiftResumeClusterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RevokeClusterSecurityGroupIngress(AwsRedshiftRevokeClusterSecurityGroupIngressOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RevokeEndpointAccess(AwsRedshiftRevokeEndpointAccessOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftRevokeEndpointAccessOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RevokeSnapshotAccess(AwsRedshiftRevokeSnapshotAccessOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RotateEncryptionKey(AwsRedshiftRotateEncryptionKeyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdatePartnerStatus(AwsRedshiftUpdatePartnerStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}