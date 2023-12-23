using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsFsx
{
    public AwsFsx(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateFileSystemAliases(AwsFsxAssociateFileSystemAliasesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelDataRepositoryTask(AwsFsxCancelDataRepositoryTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyBackup(AwsFsxCopyBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopySnapshotAndUpdateVolume(AwsFsxCopySnapshotAndUpdateVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBackup(AwsFsxCreateBackupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFsxCreateBackupOptions(), token);
    }

    public async Task<CommandResult> CreateDataRepositoryAssociation(AwsFsxCreateDataRepositoryAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataRepositoryTask(AwsFsxCreateDataRepositoryTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFileCache(AwsFsxCreateFileCacheOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFileSystem(AwsFsxCreateFileSystemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFileSystemFromBackup(AwsFsxCreateFileSystemFromBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSnapshot(AwsFsxCreateSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStorageVirtualMachine(AwsFsxCreateStorageVirtualMachineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVolume(AwsFsxCreateVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVolumeFromBackup(AwsFsxCreateVolumeFromBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBackup(AwsFsxDeleteBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDataRepositoryAssociation(AwsFsxDeleteDataRepositoryAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFileCache(AwsFsxDeleteFileCacheOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFileSystem(AwsFsxDeleteFileSystemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSnapshot(AwsFsxDeleteSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStorageVirtualMachine(AwsFsxDeleteStorageVirtualMachineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVolume(AwsFsxDeleteVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBackups(AwsFsxDescribeBackupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFsxDescribeBackupsOptions(), token);
    }

    public async Task<CommandResult> DescribeDataRepositoryAssociations(AwsFsxDescribeDataRepositoryAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFsxDescribeDataRepositoryAssociationsOptions(), token);
    }

    public async Task<CommandResult> DescribeDataRepositoryTasks(AwsFsxDescribeDataRepositoryTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFsxDescribeDataRepositoryTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeFileCaches(AwsFsxDescribeFileCachesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFsxDescribeFileCachesOptions(), token);
    }

    public async Task<CommandResult> DescribeFileSystemAliases(AwsFsxDescribeFileSystemAliasesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFileSystems(AwsFsxDescribeFileSystemsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFsxDescribeFileSystemsOptions(), token);
    }

    public async Task<CommandResult> DescribeSharedVpcConfiguration(AwsFsxDescribeSharedVpcConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFsxDescribeSharedVpcConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeSnapshots(AwsFsxDescribeSnapshotsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFsxDescribeSnapshotsOptions(), token);
    }

    public async Task<CommandResult> DescribeStorageVirtualMachines(AwsFsxDescribeStorageVirtualMachinesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFsxDescribeStorageVirtualMachinesOptions(), token);
    }

    public async Task<CommandResult> DescribeVolumes(AwsFsxDescribeVolumesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFsxDescribeVolumesOptions(), token);
    }

    public async Task<CommandResult> DisassociateFileSystemAliases(AwsFsxDisassociateFileSystemAliasesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsFsxListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReleaseFileSystemNfsV3Locks(AwsFsxReleaseFileSystemNfsV3LocksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreVolumeFromSnapshot(AwsFsxRestoreVolumeFromSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMisconfiguredStateRecovery(AwsFsxStartMisconfiguredStateRecoveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsFsxTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsFsxUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDataRepositoryAssociation(AwsFsxUpdateDataRepositoryAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFileCache(AwsFsxUpdateFileCacheOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFileSystem(AwsFsxUpdateFileSystemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSharedVpcConfiguration(AwsFsxUpdateSharedVpcConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFsxUpdateSharedVpcConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateSnapshot(AwsFsxUpdateSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateStorageVirtualMachine(AwsFsxUpdateStorageVirtualMachineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVolume(AwsFsxUpdateVolumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}