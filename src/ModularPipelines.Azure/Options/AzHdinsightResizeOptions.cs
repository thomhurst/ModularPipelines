using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight", "resize")]
public record AzHdinsightResizeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workernode-count")] int WorkernodeCount
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}