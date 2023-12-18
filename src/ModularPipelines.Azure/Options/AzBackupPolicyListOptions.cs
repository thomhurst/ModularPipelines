using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "policy", "list")]
public record AzBackupPolicyListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CommandSwitch("--move-to-archive-tier")]
    public string? MoveToArchiveTier { get; set; }

    [CommandSwitch("--policy-sub-type")]
    public string? PolicySubType { get; set; }

    [CommandSwitch("--workload-type")]
    public string? WorkloadType { get; set; }
}