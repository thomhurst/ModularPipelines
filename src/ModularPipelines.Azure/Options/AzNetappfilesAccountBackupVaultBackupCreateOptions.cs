using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "account", "backup-vault", "backup", "create")]
public record AzNetappfilesAccountBackupVaultBackupCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--backup-name")] string BackupName,
[property: CliOption("--backup-vault-name")] string BackupVaultName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--volume-resource-id")] string VolumeResourceId
) : AzOptions
{
    [CliOption("--label")]
    public string? Label { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [CliFlag("--use-existing-snapshot")]
    public bool? UseExistingSnapshot { get; set; }
}