using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "recoverypoint", "show-log-chain")]
public record AzBackupRecoverypointShowLogChainOptions(
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--item-name")] string ItemName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--use-secondary-region")]
    public string? UseSecondaryRegion { get; set; }

    [CommandSwitch("--workload-type")]
    public string? WorkloadType { get; set; }
}

