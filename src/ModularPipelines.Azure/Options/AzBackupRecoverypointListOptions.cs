using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "recoverypoint", "list")]
public record AzBackupRecoverypointListOptions(
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--item-name")] string ItemName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliFlag("--is-ready-for-move")]
    public bool? IsReadyForMove { get; set; }

    [CliOption("--recommended-for-archive")]
    public string? RecommendedForArchive { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }

    [CliOption("--target-tier")]
    public string? TargetTier { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliFlag("--use-secondary-region")]
    public bool? UseSecondaryRegion { get; set; }

    [CliOption("--workload-type")]
    public string? WorkloadType { get; set; }
}