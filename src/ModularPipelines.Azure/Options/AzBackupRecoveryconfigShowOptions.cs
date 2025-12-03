using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "recoveryconfig", "show")]
public record AzBackupRecoveryconfigShowOptions(
[property: CliOption("--restore-mode")] string RestoreMode
) : AzOptions
{
    [CliOption("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--filepath")]
    public string? Filepath { get; set; }

    [CliOption("--from-full-rp-name")]
    public string? FromFullRpName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--item-name")]
    public string? ItemName { get; set; }

    [CliOption("--log-point-in-time")]
    public string? LogPointInTime { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rp-name")]
    public string? RpName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-container-name")]
    public string? TargetContainerName { get; set; }

    [CliOption("--target-instance-name")]
    public string? TargetInstanceName { get; set; }

    [CliOption("--target-item-name")]
    public string? TargetItemName { get; set; }

    [CliOption("--target-resource-group")]
    public string? TargetResourceGroup { get; set; }

    [CliOption("--target-server-name")]
    public string? TargetServerName { get; set; }

    [CliOption("--target-server-type")]
    public string? TargetServerType { get; set; }

    [CliOption("--target-subscription-id")]
    public string? TargetSubscriptionId { get; set; }

    [CliOption("--target-vault-name")]
    public string? TargetVaultName { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }

    [CliOption("--workload-type")]
    public string? WorkloadType { get; set; }
}