using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "account", "backup-vault", "backup", "create")]
public record AzNetappfilesAccountBackupVaultBackupCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--backup-name")] string BackupName,
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--volume-resource-id")] string VolumeResourceId
) : AzOptions
{
    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [BooleanCommandSwitch("--use-existing-snapshot")]
    public bool? UseExistingSnapshot { get; set; }
}