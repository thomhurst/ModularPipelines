using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "item", "set-policy")]
public record AzBackupItemSetPolicyOptions(
[property: CliOption("--policy-name")] string PolicyName
) : AzOptions
{
    [CliOption("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }

    [CliOption("--workload-type")]
    public string? WorkloadType { get; set; }
}