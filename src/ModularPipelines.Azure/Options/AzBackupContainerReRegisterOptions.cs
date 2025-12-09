using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "container", "re-register")]
public record AzBackupContainerReRegisterOptions(
[property: CliOption("--workload-type")] string WorkloadType
) : AzOptions
{
    [CliOption("--backup-management-type")]
    public string? BackupManagementType { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}