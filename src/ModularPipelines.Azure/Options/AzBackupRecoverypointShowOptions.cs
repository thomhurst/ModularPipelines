using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "recoverypoint", "show")]
public record AzBackupRecoverypointShowOptions(
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--item-name")] string ItemName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--use-secondary-region")]
    public string? UseSecondaryRegion { get; set; }

    [CommandSwitch("--workload-type")]
    public string? WorkloadType { get; set; }
}