using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "policy", "create")]
public record AzBackupPolicyCreateOptions(
[property: CommandSwitch("--backup-management-type")] string BackupManagementType,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy")] string Policy,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--workload-type")]
    public string? WorkloadType { get; set; }
}

