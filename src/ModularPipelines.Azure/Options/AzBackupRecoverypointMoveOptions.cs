using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "recoverypoint", "move")]
public record AzBackupRecoverypointMoveOptions(
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--destination-tier")] string DestinationTier,
[property: CommandSwitch("--item-name")] string ItemName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--source-tier")] string SourceTier,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CommandSwitch("--workload-type")]
    public string? WorkloadType { get; set; }
}