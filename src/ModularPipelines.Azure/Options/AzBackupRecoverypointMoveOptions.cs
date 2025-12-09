using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "recoverypoint", "move")]
public record AzBackupRecoverypointMoveOptions(
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--destination-tier")] string DestinationTier,
[property: CliOption("--item-name")] string ItemName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source-tier")] string SourceTier,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CliOption("--workload-type")]
    public string? WorkloadType { get; set; }
}