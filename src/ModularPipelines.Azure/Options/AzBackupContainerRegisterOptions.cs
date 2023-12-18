using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "container", "register")]
public record AzBackupContainerRegisterOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--vault-name")] string VaultName,
[property: CommandSwitch("--workload-type")] string WorkloadType
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }
}