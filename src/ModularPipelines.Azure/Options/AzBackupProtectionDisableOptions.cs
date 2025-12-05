using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "protection", "disable")]
public record AzBackupProtectionDisableOptions : AzOptions
{
    [CliOption("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliFlag("--delete-backup-data")]
    public bool? DeleteBackupData { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--item-name")]
    public string? ItemName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--retain-as-per-policy")]
    public bool? RetainAsPerPolicy { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }

    [CliOption("--workload-type")]
    public string? WorkloadType { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}