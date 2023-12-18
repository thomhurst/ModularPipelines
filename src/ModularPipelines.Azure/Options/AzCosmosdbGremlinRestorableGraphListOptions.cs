using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "gremlin", "restorable-graph", "list")]
public record AzCosmosdbGremlinRestorableGraphListOptions(
[property: CommandSwitch("--database-rid")] string DatabaseRid,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}