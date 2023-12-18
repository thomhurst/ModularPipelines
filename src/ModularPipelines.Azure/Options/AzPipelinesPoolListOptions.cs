using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "pool", "list")]
public record AzPipelinesPoolListOptions : AzOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--pool-name")]
    public string? PoolName { get; set; }

    [CommandSwitch("--pool-type")]
    public string? PoolType { get; set; }
}