using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "item", "list")]
public record AzBackupItemListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [BooleanCommandSwitch("--use-secondary-region")]
    public bool? UseSecondaryRegion { get; set; }

    [CommandSwitch("--workload-type")]
    public string? WorkloadType { get; set; }
}