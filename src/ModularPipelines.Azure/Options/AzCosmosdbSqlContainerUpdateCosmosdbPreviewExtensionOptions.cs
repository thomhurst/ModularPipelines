using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "container", "update", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbSqlContainerUpdateCosmosdbPreviewExtensionOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--analytical-storage-ttl")]
    public string? AnalyticalStorageTtl { get; set; }

    [CommandSwitch("--idx")]
    public string? Idx { get; set; }

    [CommandSwitch("--materialized-view-definition")]
    public string? MaterializedViewDefinition { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }
}