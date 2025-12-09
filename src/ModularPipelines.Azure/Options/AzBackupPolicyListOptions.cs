using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "policy", "list")]
public record AzBackupPolicyListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CliOption("--move-to-archive-tier")]
    public string? MoveToArchiveTier { get; set; }

    [CliOption("--policy-sub-type")]
    public string? PolicySubType { get; set; }

    [CliOption("--workload-type")]
    public string? WorkloadType { get; set; }
}