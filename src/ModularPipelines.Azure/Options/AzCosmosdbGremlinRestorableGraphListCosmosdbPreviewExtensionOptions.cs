using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "gremlin", "restorable-graph", "list", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbGremlinRestorableGraphListCosmosdbPreviewExtensionOptions(
[property: CliOption("--database-rid")] string DatabaseRid,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}