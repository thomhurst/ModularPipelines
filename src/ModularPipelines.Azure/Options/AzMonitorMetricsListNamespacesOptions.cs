using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "metrics", "list-namespaces")]
public record AzMonitorMetricsListNamespacesOptions(
[property: CommandSwitch("--resource-uri")] string ResourceUri
) : AzOptions
{
    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}