using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "policy", "create")]
public record AzBackupPolicyCreateOptions(
[property: CliOption("--backup-management-type")] string BackupManagementType,
[property: CliOption("--name")] string Name,
[property: CliOption("--policy")] string Policy,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--workload-type")]
    public string? WorkloadType { get; set; }
}