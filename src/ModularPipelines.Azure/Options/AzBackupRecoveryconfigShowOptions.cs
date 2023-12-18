using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "recoveryconfig", "show")]
public record AzBackupRecoveryconfigShowOptions(
[property: CommandSwitch("--restore-mode")] string RestoreMode
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--filepath")]
    public string? Filepath { get; set; }

    [CommandSwitch("--from-full-rp-name")]
    public string? FromFullRpName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--item-name")]
    public string? ItemName { get; set; }

    [CommandSwitch("--log-point-in-time")]
    public string? LogPointInTime { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rp-name")]
    public string? RpName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--target-container-name")]
    public string? TargetContainerName { get; set; }

    [CommandSwitch("--target-instance-name")]
    public string? TargetInstanceName { get; set; }

    [CommandSwitch("--target-item-name")]
    public string? TargetItemName { get; set; }

    [CommandSwitch("--target-resource-group")]
    public string? TargetResourceGroup { get; set; }

    [CommandSwitch("--target-server-name")]
    public string? TargetServerName { get; set; }

    [CommandSwitch("--target-server-type")]
    public string? TargetServerType { get; set; }

    [CommandSwitch("--target-subscription-id")]
    public string? TargetSubscriptionId { get; set; }

    [CommandSwitch("--target-vault-name")]
    public string? TargetVaultName { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }

    [CommandSwitch("--workload-type")]
    public string? WorkloadType { get; set; }
}