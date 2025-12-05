using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "table", "restorable-table", "list")]
public record AzCosmosdbTableRestorableTableListOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}