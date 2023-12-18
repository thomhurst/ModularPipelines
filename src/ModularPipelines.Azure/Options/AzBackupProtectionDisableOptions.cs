using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "protection", "disable")]
public record AzBackupProtectionDisableOptions(
[property: CommandSwitch("--azure-file-share")] string AzureFileShare,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-account")] int StorageAccount,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [BooleanCommandSwitch("--delete-backup-data")]
    public bool? DeleteBackupData { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--item-name")]
    public string? ItemName { get; set; }

    [BooleanCommandSwitch("--retain-as-per-policy")]
    public bool? RetainAsPerPolicy { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }

    [CommandSwitch("--workload-type")]
    public string? WorkloadType { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}