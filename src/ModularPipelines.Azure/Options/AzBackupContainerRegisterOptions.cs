using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "container", "register")]
public record AzBackupContainerRegisterOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--vault-name")] string VaultName,
[property: CliOption("--workload-type")] string WorkloadType
) : AzOptions
{
    [CliOption("--backup-management-type")]
    public string? BackupManagementType { get; set; }
}