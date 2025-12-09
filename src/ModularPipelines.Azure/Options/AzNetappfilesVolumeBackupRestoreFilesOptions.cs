using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "volume", "backup", "restore-files")]
public record AzNetappfilesVolumeBackupRestoreFilesOptions(
[property: CliOption("--destination-path")] string DestinationPath,
[property: CliOption("--destination-volume-id")] string DestinationVolumeId,
[property: CliOption("--file-paths")] string FilePaths
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--backup-name")]
    public string? BackupName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pool-name")]
    public string? PoolName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}