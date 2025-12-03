using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipelines", "pool", "list")]
public record AzPipelinesPoolListOptions : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--pool-name")]
    public string? PoolName { get; set; }

    [CliOption("--pool-type")]
    public string? PoolType { get; set; }
}