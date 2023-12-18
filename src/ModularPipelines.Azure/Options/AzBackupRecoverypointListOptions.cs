using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "recoverypoint", "list")]
public record AzBackupRecoverypointListOptions(
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--item-name")] string ItemName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [BooleanCommandSwitch("--is-ready-for-move")]
    public bool? IsReadyForMove { get; set; }

    [CommandSwitch("--recommended-for-archive")]
    public string? RecommendedForArchive { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--target-tier")]
    public string? TargetTier { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--use-secondary-region")]
    public string? UseSecondaryRegion { get; set; }

    [CommandSwitch("--workload-type")]
    public string? WorkloadType { get; set; }
}