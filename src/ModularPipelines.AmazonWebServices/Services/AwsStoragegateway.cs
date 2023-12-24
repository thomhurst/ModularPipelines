using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsStoragegateway
{
    public AwsStoragegateway(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ActivateGateway(AwsStoragegatewayActivateGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddCache(AwsStoragegatewayAddCacheOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddTagsToResource(AwsStoragegatewayAddTagsToResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddUploadBuffer(AwsStoragegatewayAddUploadBufferOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddWorkingStorage(AwsStoragegatewayAddWorkingStorageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssignTapePool(AwsStoragegatewayAssignTapePoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateFileSystem(AwsStoragegatewayAssociateFileSystemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachVolume(AwsStoragegatewayAttachVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelArchival(AwsStoragegatewayCancelArchivalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelRetrieval(AwsStoragegatewayCancelRetrievalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCachedIscsiVolume(AwsStoragegatewayCreateCachedIscsiVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNfsFileShare(AwsStoragegatewayCreateNfsFileShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSmbFileShare(AwsStoragegatewayCreateSmbFileShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSnapshot(AwsStoragegatewayCreateSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSnapshotFromVolumeRecoveryPoint(AwsStoragegatewayCreateSnapshotFromVolumeRecoveryPointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStoredIscsiVolume(AwsStoragegatewayCreateStoredIscsiVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTapePool(AwsStoragegatewayCreateTapePoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTapeWithBarcode(AwsStoragegatewayCreateTapeWithBarcodeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTapes(AwsStoragegatewayCreateTapesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAutomaticTapeCreationPolicy(AwsStoragegatewayDeleteAutomaticTapeCreationPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBandwidthRateLimit(AwsStoragegatewayDeleteBandwidthRateLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteChapCredentials(AwsStoragegatewayDeleteChapCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFileShare(AwsStoragegatewayDeleteFileShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGateway(AwsStoragegatewayDeleteGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSnapshotSchedule(AwsStoragegatewayDeleteSnapshotScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTape(AwsStoragegatewayDeleteTapeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTapeArchive(AwsStoragegatewayDeleteTapeArchiveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTapePool(AwsStoragegatewayDeleteTapePoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVolume(AwsStoragegatewayDeleteVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAvailabilityMonitorTest(AwsStoragegatewayDescribeAvailabilityMonitorTestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBandwidthRateLimit(AwsStoragegatewayDescribeBandwidthRateLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBandwidthRateLimitSchedule(AwsStoragegatewayDescribeBandwidthRateLimitScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCache(AwsStoragegatewayDescribeCacheOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCachedIscsiVolumes(AwsStoragegatewayDescribeCachedIscsiVolumesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeChapCredentials(AwsStoragegatewayDescribeChapCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFileSystemAssociations(AwsStoragegatewayDescribeFileSystemAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeGatewayInformation(AwsStoragegatewayDescribeGatewayInformationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMaintenanceStartTime(AwsStoragegatewayDescribeMaintenanceStartTimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeNfsFileShares(AwsStoragegatewayDescribeNfsFileSharesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSmbFileShares(AwsStoragegatewayDescribeSmbFileSharesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSmbSettings(AwsStoragegatewayDescribeSmbSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSnapshotSchedule(AwsStoragegatewayDescribeSnapshotScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStoredIscsiVolumes(AwsStoragegatewayDescribeStoredIscsiVolumesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTapeArchives(AwsStoragegatewayDescribeTapeArchivesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsStoragegatewayDescribeTapeArchivesOptions(), token);
    }

    public async Task<CommandResult> DescribeTapeRecoveryPoints(AwsStoragegatewayDescribeTapeRecoveryPointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTapes(AwsStoragegatewayDescribeTapesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeUploadBuffer(AwsStoragegatewayDescribeUploadBufferOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeVtlDevices(AwsStoragegatewayDescribeVtlDevicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWorkingStorage(AwsStoragegatewayDescribeWorkingStorageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachVolume(AwsStoragegatewayDetachVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableGateway(AwsStoragegatewayDisableGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateFileSystem(AwsStoragegatewayDisassociateFileSystemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> JoinDomain(AwsStoragegatewayJoinDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAutomaticTapeCreationPolicies(AwsStoragegatewayListAutomaticTapeCreationPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsStoragegatewayListAutomaticTapeCreationPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListFileShares(AwsStoragegatewayListFileSharesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsStoragegatewayListFileSharesOptions(), token);
    }

    public async Task<CommandResult> ListFileSystemAssociations(AwsStoragegatewayListFileSystemAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsStoragegatewayListFileSystemAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListGateways(AwsStoragegatewayListGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsStoragegatewayListGatewaysOptions(), token);
    }

    public async Task<CommandResult> ListLocalDisks(AwsStoragegatewayListLocalDisksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsStoragegatewayListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTapePools(AwsStoragegatewayListTapePoolsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsStoragegatewayListTapePoolsOptions(), token);
    }

    public async Task<CommandResult> ListTapes(AwsStoragegatewayListTapesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsStoragegatewayListTapesOptions(), token);
    }

    public async Task<CommandResult> ListVolumeInitiators(AwsStoragegatewayListVolumeInitiatorsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVolumeRecoveryPoints(AwsStoragegatewayListVolumeRecoveryPointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVolumes(AwsStoragegatewayListVolumesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsStoragegatewayListVolumesOptions(), token);
    }

    public async Task<CommandResult> NotifyWhenUploaded(AwsStoragegatewayNotifyWhenUploadedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RefreshCache(AwsStoragegatewayRefreshCacheOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTagsFromResource(AwsStoragegatewayRemoveTagsFromResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetCache(AwsStoragegatewayResetCacheOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RetrieveTapeArchive(AwsStoragegatewayRetrieveTapeArchiveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RetrieveTapeRecoveryPoint(AwsStoragegatewayRetrieveTapeRecoveryPointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetLocalConsolePassword(AwsStoragegatewaySetLocalConsolePasswordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetSmbGuestPassword(AwsStoragegatewaySetSmbGuestPasswordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShutdownGateway(AwsStoragegatewayShutdownGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartAvailabilityMonitorTest(AwsStoragegatewayStartAvailabilityMonitorTestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartGateway(AwsStoragegatewayStartGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAutomaticTapeCreationPolicy(AwsStoragegatewayUpdateAutomaticTapeCreationPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBandwidthRateLimit(AwsStoragegatewayUpdateBandwidthRateLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBandwidthRateLimitSchedule(AwsStoragegatewayUpdateBandwidthRateLimitScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateChapCredentials(AwsStoragegatewayUpdateChapCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFileSystemAssociation(AwsStoragegatewayUpdateFileSystemAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGatewayInformation(AwsStoragegatewayUpdateGatewayInformationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGatewaySoftwareNow(AwsStoragegatewayUpdateGatewaySoftwareNowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMaintenanceStartTime(AwsStoragegatewayUpdateMaintenanceStartTimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateNfsFileShare(AwsStoragegatewayUpdateNfsFileShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSmbFileShare(AwsStoragegatewayUpdateSmbFileShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSmbFileShareVisibility(AwsStoragegatewayUpdateSmbFileShareVisibilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSmbLocalGroups(AwsStoragegatewayUpdateSmbLocalGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSmbSecurityStrategy(AwsStoragegatewayUpdateSmbSecurityStrategyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSnapshotSchedule(AwsStoragegatewayUpdateSnapshotScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVtlDeviceType(AwsStoragegatewayUpdateVtlDeviceTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}