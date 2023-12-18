using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "account", "backup-vault", "backup", "restore-file")]
public record AzNetappfilesAccountBackupVaultBackupRestoreFileOptions(
[property: CommandSwitch("--destination-volume-id")] string DestinationVolumeId,
[property: CommandSwitch("--file-list")] string FileList
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--backup-name")]
    public string? BackupName { get; set; }

    [CommandSwitch("--backup-vault-name")]
    public string? BackupVaultName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--restore-file-path")]
    public string? RestoreFilePath { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

