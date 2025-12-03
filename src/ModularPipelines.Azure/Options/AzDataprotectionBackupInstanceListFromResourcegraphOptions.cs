using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataprotection", "backup-instance", "list-from-resourcegraph")]
public record AzDataprotectionBackupInstanceListFromResourcegraphOptions(
[property: CliOption("--datasource-type")] string DatasourceType
) : AzOptions
{
    [CliOption("--datasource-id")]
    public string? DatasourceId { get; set; }

    [CliOption("--protection-status")]
    public string? ProtectionStatus { get; set; }

    [CliOption("--resource-groups")]
    public string? ResourceGroups { get; set; }

    [CliOption("--subscriptions")]
    public string? Subscriptions { get; set; }

    [CliOption("--vaults")]
    public string? Vaults { get; set; }
}