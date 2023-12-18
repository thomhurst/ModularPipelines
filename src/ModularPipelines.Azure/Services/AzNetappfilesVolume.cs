using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles")]
public class AzNetappfilesVolume
{
    public AzNetappfilesVolume(
        AzNetappfilesVolumeBackup backup,
        AzNetappfilesVolumeCreate create,
        AzNetappfilesVolumeExportPolicy exportPolicy,
        AzNetappfilesVolumeLatestBackupStatus latestBackupStatus,
        AzNetappfilesVolumeList list,
        AzNetappfilesVolumeQuotaRule quotaRule,
        AzNetappfilesVolumeReplication replication,
        AzNetappfilesVolumeShow show,
        AzNetappfilesVolumeUpdate update,
        AzNetappfilesVolumeWait wait,
        ICommand internalCommand
    )
    {
        Backup = backup;
        CreateCommands = create;
        ExportPolicy = exportPolicy;
        LatestBackupStatus = latestBackupStatus;
        ListCommands = list;
        QuotaRule = quotaRule;
        Replication = replication;
        ShowCommands = show;
        UpdateCommands = update;
        WaitCommands = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetappfilesVolumeBackup Backup { get; }

    public AzNetappfilesVolumeCreate CreateCommands { get; }

    public AzNetappfilesVolumeExportPolicy ExportPolicy { get; }

    public AzNetappfilesVolumeLatestBackupStatus LatestBackupStatus { get; }

    public AzNetappfilesVolumeList ListCommands { get; }

    public AzNetappfilesVolumeQuotaRule QuotaRule { get; }

    public AzNetappfilesVolumeReplication Replication { get; }

    public AzNetappfilesVolumeShow ShowCommands { get; }

    public AzNetappfilesVolumeUpdate UpdateCommands { get; }

    public AzNetappfilesVolumeWait WaitCommands { get; }

    public async Task<CommandResult> BreakFileLocks(AzNetappfilesVolumeBreakFileLocksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNetappfilesVolumeCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetappfilesVolumeDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FinalizeRelocation(AzNetappfilesVolumeFinalizeRelocationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGroupidListForLdapuser(AzNetappfilesVolumeGetGroupidListForLdapuserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetappfilesVolumeListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MigrateBackup(AzNetappfilesVolumeMigrateBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PoolChange(AzNetappfilesVolumePoolChangeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Relocate(AzNetappfilesVolumeRelocateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetCifsPw(AzNetappfilesVolumeResetCifsPwOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Revert(AzNetappfilesVolumeRevertOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevertRelocation(AzNetappfilesVolumeRevertRelocationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeRevertRelocationOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetappfilesVolumeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetappfilesVolumeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetappfilesVolumeWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeWaitOptions(), token);
    }
}

