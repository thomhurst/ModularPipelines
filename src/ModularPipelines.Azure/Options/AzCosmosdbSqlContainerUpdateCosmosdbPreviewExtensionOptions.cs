using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "sql", "container", "update", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbSqlContainerUpdateCosmosdbPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--analytical-storage-ttl")]
    public string? AnalyticalStorageTtl { get; set; }

    [CliOption("--idx")]
    public string? Idx { get; set; }

    [CliOption("--materialized-view-definition")]
    public string? MaterializedViewDefinition { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}