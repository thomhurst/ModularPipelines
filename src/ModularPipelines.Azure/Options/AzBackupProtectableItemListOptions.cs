using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "protectable-item", "list")]
public record AzBackupProtectableItemListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName,
[property: CliOption("--workload-type")] string WorkloadType
) : AzOptions
{
    [CliOption("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--protectable-item-type")]
    public string? ProtectableItemType { get; set; }

    [CliOption("--server-name")]
    public string? ServerName { get; set; }
}