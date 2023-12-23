using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsLightsail
{
    public AwsLightsail(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AllocateStaticIp(AwsLightsailAllocateStaticIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachCertificateToDistribution(AwsLightsailAttachCertificateToDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachDisk(AwsLightsailAttachDiskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachInstancesToLoadBalancer(AwsLightsailAttachInstancesToLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachLoadBalancerTlsCertificate(AwsLightsailAttachLoadBalancerTlsCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachStaticIp(AwsLightsailAttachStaticIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CloseInstancePublicPorts(AwsLightsailCloseInstancePublicPortsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopySnapshot(AwsLightsailCopySnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBucket(AwsLightsailCreateBucketOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBucketAccessKey(AwsLightsailCreateBucketAccessKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCertificate(AwsLightsailCreateCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCloudFormationStack(AwsLightsailCreateCloudFormationStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateContactMethod(AwsLightsailCreateContactMethodOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateContainerService(AwsLightsailCreateContainerServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateContainerServiceDeployment(AwsLightsailCreateContainerServiceDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateContainerServiceRegistryLogin(AwsLightsailCreateContainerServiceRegistryLoginOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailCreateContainerServiceRegistryLoginOptions(), token);
    }

    public async Task<CommandResult> CreateDisk(AwsLightsailCreateDiskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDiskFromSnapshot(AwsLightsailCreateDiskFromSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDiskSnapshot(AwsLightsailCreateDiskSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDistribution(AwsLightsailCreateDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDomain(AwsLightsailCreateDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDomainEntry(AwsLightsailCreateDomainEntryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGuiSessionAccessDetails(AwsLightsailCreateGuiSessionAccessDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInstanceSnapshot(AwsLightsailCreateInstanceSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInstances(AwsLightsailCreateInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInstancesFromSnapshot(AwsLightsailCreateInstancesFromSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateKeyPair(AwsLightsailCreateKeyPairOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLoadBalancer(AwsLightsailCreateLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLoadBalancerTlsCertificate(AwsLightsailCreateLoadBalancerTlsCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRelationalDatabase(AwsLightsailCreateRelationalDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRelationalDatabaseFromSnapshot(AwsLightsailCreateRelationalDatabaseFromSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRelationalDatabaseSnapshot(AwsLightsailCreateRelationalDatabaseSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAlarm(AwsLightsailDeleteAlarmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAutoSnapshot(AwsLightsailDeleteAutoSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBucket(AwsLightsailDeleteBucketOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBucketAccessKey(AwsLightsailDeleteBucketAccessKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCertificate(AwsLightsailDeleteCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteContactMethod(AwsLightsailDeleteContactMethodOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteContainerImage(AwsLightsailDeleteContainerImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteContainerService(AwsLightsailDeleteContainerServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDisk(AwsLightsailDeleteDiskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDiskSnapshot(AwsLightsailDeleteDiskSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDistribution(AwsLightsailDeleteDistributionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailDeleteDistributionOptions(), token);
    }

    public async Task<CommandResult> DeleteDomain(AwsLightsailDeleteDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDomainEntry(AwsLightsailDeleteDomainEntryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstance(AwsLightsailDeleteInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstanceSnapshot(AwsLightsailDeleteInstanceSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteKeyPair(AwsLightsailDeleteKeyPairOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteKnownHostKeys(AwsLightsailDeleteKnownHostKeysOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLoadBalancer(AwsLightsailDeleteLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLoadBalancerTlsCertificate(AwsLightsailDeleteLoadBalancerTlsCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRelationalDatabase(AwsLightsailDeleteRelationalDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRelationalDatabaseSnapshot(AwsLightsailDeleteRelationalDatabaseSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachCertificateFromDistribution(AwsLightsailDetachCertificateFromDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachDisk(AwsLightsailDetachDiskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachInstancesFromLoadBalancer(AwsLightsailDetachInstancesFromLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachStaticIp(AwsLightsailDetachStaticIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableAddOn(AwsLightsailDisableAddOnOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DownloadDefaultKeyPair(AwsLightsailDownloadDefaultKeyPairOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailDownloadDefaultKeyPairOptions(), token);
    }

    public async Task<CommandResult> EnableAddOn(AwsLightsailEnableAddOnOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportSnapshot(AwsLightsailExportSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetActiveNames(AwsLightsailGetActiveNamesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetActiveNamesOptions(), token);
    }

    public async Task<CommandResult> GetAlarms(AwsLightsailGetAlarmsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetAlarmsOptions(), token);
    }

    public async Task<CommandResult> GetAutoSnapshots(AwsLightsailGetAutoSnapshotsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBlueprints(AwsLightsailGetBlueprintsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetBlueprintsOptions(), token);
    }

    public async Task<CommandResult> GetBucketAccessKeys(AwsLightsailGetBucketAccessKeysOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBucketBundles(AwsLightsailGetBucketBundlesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetBucketBundlesOptions(), token);
    }

    public async Task<CommandResult> GetBucketMetricData(AwsLightsailGetBucketMetricDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBuckets(AwsLightsailGetBucketsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetBucketsOptions(), token);
    }

    public async Task<CommandResult> GetBundles(AwsLightsailGetBundlesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetBundlesOptions(), token);
    }

    public async Task<CommandResult> GetCertificates(AwsLightsailGetCertificatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetCertificatesOptions(), token);
    }

    public async Task<CommandResult> GetCloudFormationStackRecords(AwsLightsailGetCloudFormationStackRecordsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetCloudFormationStackRecordsOptions(), token);
    }

    public async Task<CommandResult> GetContactMethods(AwsLightsailGetContactMethodsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetContactMethodsOptions(), token);
    }

    public async Task<CommandResult> GetContainerApiMetadata(AwsLightsailGetContainerApiMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetContainerApiMetadataOptions(), token);
    }

    public async Task<CommandResult> GetContainerImages(AwsLightsailGetContainerImagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContainerLog(AwsLightsailGetContainerLogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContainerServiceDeployments(AwsLightsailGetContainerServiceDeploymentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContainerServiceMetricData(AwsLightsailGetContainerServiceMetricDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContainerServicePowers(AwsLightsailGetContainerServicePowersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetContainerServicePowersOptions(), token);
    }

    public async Task<CommandResult> GetContainerServices(AwsLightsailGetContainerServicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetContainerServicesOptions(), token);
    }

    public async Task<CommandResult> GetCostEstimate(AwsLightsailGetCostEstimateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDisk(AwsLightsailGetDiskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDiskSnapshot(AwsLightsailGetDiskSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDiskSnapshots(AwsLightsailGetDiskSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetDiskSnapshotsOptions(), token);
    }

    public async Task<CommandResult> GetDisks(AwsLightsailGetDisksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetDisksOptions(), token);
    }

    public async Task<CommandResult> GetDistributionBundles(AwsLightsailGetDistributionBundlesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetDistributionBundlesOptions(), token);
    }

    public async Task<CommandResult> GetDistributionLatestCacheReset(AwsLightsailGetDistributionLatestCacheResetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetDistributionLatestCacheResetOptions(), token);
    }

    public async Task<CommandResult> GetDistributionMetricData(AwsLightsailGetDistributionMetricDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDistributions(AwsLightsailGetDistributionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetDistributionsOptions(), token);
    }

    public async Task<CommandResult> GetDomain(AwsLightsailGetDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDomains(AwsLightsailGetDomainsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetDomainsOptions(), token);
    }

    public async Task<CommandResult> GetExportSnapshotRecords(AwsLightsailGetExportSnapshotRecordsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetExportSnapshotRecordsOptions(), token);
    }

    public async Task<CommandResult> GetInstance(AwsLightsailGetInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInstanceAccessDetails(AwsLightsailGetInstanceAccessDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInstanceMetricData(AwsLightsailGetInstanceMetricDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInstancePortStates(AwsLightsailGetInstancePortStatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInstanceSnapshot(AwsLightsailGetInstanceSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInstanceSnapshots(AwsLightsailGetInstanceSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetInstanceSnapshotsOptions(), token);
    }

    public async Task<CommandResult> GetInstanceState(AwsLightsailGetInstanceStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInstances(AwsLightsailGetInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetInstancesOptions(), token);
    }

    public async Task<CommandResult> GetKeyPair(AwsLightsailGetKeyPairOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetKeyPairs(AwsLightsailGetKeyPairsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetKeyPairsOptions(), token);
    }

    public async Task<CommandResult> GetLoadBalancer(AwsLightsailGetLoadBalancerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLoadBalancerMetricData(AwsLightsailGetLoadBalancerMetricDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLoadBalancerTlsCertificates(AwsLightsailGetLoadBalancerTlsCertificatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLoadBalancerTlsPolicies(AwsLightsailGetLoadBalancerTlsPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetLoadBalancerTlsPoliciesOptions(), token);
    }

    public async Task<CommandResult> GetLoadBalancers(AwsLightsailGetLoadBalancersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetLoadBalancersOptions(), token);
    }

    public async Task<CommandResult> GetOperation(AwsLightsailGetOperationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOperations(AwsLightsailGetOperationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetOperationsOptions(), token);
    }

    public async Task<CommandResult> GetOperationsForResource(AwsLightsailGetOperationsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRegions(AwsLightsailGetRegionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetRegionsOptions(), token);
    }

    public async Task<CommandResult> GetRelationalDatabase(AwsLightsailGetRelationalDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRelationalDatabaseBlueprints(AwsLightsailGetRelationalDatabaseBlueprintsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetRelationalDatabaseBlueprintsOptions(), token);
    }

    public async Task<CommandResult> GetRelationalDatabaseBundles(AwsLightsailGetRelationalDatabaseBundlesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetRelationalDatabaseBundlesOptions(), token);
    }

    public async Task<CommandResult> GetRelationalDatabaseEvents(AwsLightsailGetRelationalDatabaseEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRelationalDatabaseLogEvents(AwsLightsailGetRelationalDatabaseLogEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRelationalDatabaseLogStreams(AwsLightsailGetRelationalDatabaseLogStreamsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRelationalDatabaseMasterUserPassword(AwsLightsailGetRelationalDatabaseMasterUserPasswordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRelationalDatabaseMetricData(AwsLightsailGetRelationalDatabaseMetricDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRelationalDatabaseParameters(AwsLightsailGetRelationalDatabaseParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRelationalDatabaseSnapshot(AwsLightsailGetRelationalDatabaseSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRelationalDatabaseSnapshots(AwsLightsailGetRelationalDatabaseSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetRelationalDatabaseSnapshotsOptions(), token);
    }

    public async Task<CommandResult> GetRelationalDatabases(AwsLightsailGetRelationalDatabasesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetRelationalDatabasesOptions(), token);
    }

    public async Task<CommandResult> GetStaticIp(AwsLightsailGetStaticIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStaticIps(AwsLightsailGetStaticIpsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailGetStaticIpsOptions(), token);
    }

    public async Task<CommandResult> ImportKeyPair(AwsLightsailImportKeyPairOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> IsVpcPeered(AwsLightsailIsVpcPeeredOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailIsVpcPeeredOptions(), token);
    }

    public async Task<CommandResult> OpenInstancePublicPorts(AwsLightsailOpenInstancePublicPortsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PeerVpc(AwsLightsailPeerVpcOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailPeerVpcOptions(), token);
    }

    public async Task<CommandResult> PushContainerImage(AwsLightsailPushContainerImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAlarm(AwsLightsailPutAlarmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutInstancePublicPorts(AwsLightsailPutInstancePublicPortsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootInstance(AwsLightsailRebootInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootRelationalDatabase(AwsLightsailRebootRelationalDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterContainerImage(AwsLightsailRegisterContainerImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReleaseStaticIp(AwsLightsailReleaseStaticIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetDistributionCache(AwsLightsailResetDistributionCacheOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailResetDistributionCacheOptions(), token);
    }

    public async Task<CommandResult> SendContactMethodVerification(AwsLightsailSendContactMethodVerificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIpAddressType(AwsLightsailSetIpAddressTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetResourceAccessForBucket(AwsLightsailSetResourceAccessForBucketOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartGuiSession(AwsLightsailStartGuiSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartInstance(AwsLightsailStartInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartRelationalDatabase(AwsLightsailStartRelationalDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopGuiSession(AwsLightsailStopGuiSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopInstance(AwsLightsailStopInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopRelationalDatabase(AwsLightsailStopRelationalDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsLightsailTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestAlarm(AwsLightsailTestAlarmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnpeerVpc(AwsLightsailUnpeerVpcOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailUnpeerVpcOptions(), token);
    }

    public async Task<CommandResult> UntagResource(AwsLightsailUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBucket(AwsLightsailUpdateBucketOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBucketBundle(AwsLightsailUpdateBucketBundleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateContainerService(AwsLightsailUpdateContainerServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDistribution(AwsLightsailUpdateDistributionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDistributionBundle(AwsLightsailUpdateDistributionBundleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLightsailUpdateDistributionBundleOptions(), token);
    }

    public async Task<CommandResult> UpdateDomainEntry(AwsLightsailUpdateDomainEntryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInstanceMetadataOptions(AwsLightsailUpdateInstanceMetadataOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLoadBalancerAttribute(AwsLightsailUpdateLoadBalancerAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRelationalDatabase(AwsLightsailUpdateRelationalDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRelationalDatabaseParameters(AwsLightsailUpdateRelationalDatabaseParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}