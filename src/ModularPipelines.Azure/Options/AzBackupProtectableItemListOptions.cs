using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "protectable-item", "list")]
public record AzBackupProtectableItemListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName,
[property: CommandSwitch("--workload-type")] string WorkloadType
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--protectable-item-type")]
    public string? ProtectableItemType { get; set; }

    [CommandSwitch("--server-name")]
    public string? ServerName { get; set; }
}

