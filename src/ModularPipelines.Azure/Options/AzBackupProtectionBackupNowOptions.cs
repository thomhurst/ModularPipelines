using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "protection", "backup-now")]
public record AzBackupProtectionBackupNowOptions : AzOptions
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

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--retain-until")]
    public string? RetainUntil { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }

    [CommandSwitch("--workload-type")]
    public string? WorkloadType { get; set; }
}