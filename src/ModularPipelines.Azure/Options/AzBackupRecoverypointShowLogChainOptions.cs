using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "recoverypoint", "show-log-chain")]
public record AzBackupRecoverypointShowLogChainOptions(
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

    [CliOption("--start-date")]
    public string? StartDate { get; set; }

    [CliFlag("--use-secondary-region")]
    public bool? UseSecondaryRegion { get; set; }

    [CliOption("--workload-type")]
    public string? WorkloadType { get; set; }
}