using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "protectable-item", "show")]
public record AzBackupProtectableItemShowOptions(
[property: CliOption("--protectable-item-type")] string ProtectableItemType,
[property: CliOption("--server-name")] string ServerName,
[property: CliOption("--workload-type")] string WorkloadType
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}