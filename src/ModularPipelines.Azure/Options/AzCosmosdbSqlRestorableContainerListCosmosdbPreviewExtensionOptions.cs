using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "restorable-container", "list", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbSqlRestorableContainerListCosmosdbPreviewExtensionOptions(
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