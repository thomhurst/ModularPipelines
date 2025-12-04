using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "item", "list")]
public record AzBackupItemListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliFlag("--use-secondary-region")]
    public bool? UseSecondaryRegion { get; set; }

    [CliOption("--workload-type")]
    public string? WorkloadType { get; set; }
}