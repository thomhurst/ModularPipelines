using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "volume", "backup", "create")]
public record AzNetappfilesVolumeBackupCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--backup-name")] string BackupName,
[property: CliOption("--name")] string Name,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--use-existing-snapshot")]
    public bool? UseExistingSnapshot { get; set; }
}