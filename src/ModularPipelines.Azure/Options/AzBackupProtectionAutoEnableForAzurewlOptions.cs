using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "protection", "auto-enable-for-urewl")]
public record AzBackupProtectionAutoEnableForAzurewlOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--protectable-item-name")] string ProtectableItemName,
[property: CommandSwitch("--protectable-item-type")] string ProtectableItemType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server-name")] string ServerName,
[property: CommandSwitch("--vault-name")] string VaultName,
[property: CommandSwitch("--workload-type")] string WorkloadType
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CommandSwitch("--backup-type")]
    public string? BackupType { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [BooleanCommandSwitch("--enable-compression")]
    public bool? EnableCompression { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--item-name")]
    public string? ItemName { get; set; }

    [CommandSwitch("--retain-until")]
    public string? RetainUntil { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}