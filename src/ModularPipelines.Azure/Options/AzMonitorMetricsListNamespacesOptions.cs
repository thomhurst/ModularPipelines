using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "metrics", "list-namespaces")]
public record AzMonitorMetricsListNamespacesOptions(
[property: CommandSwitch("--resource-uri")] string ResourceUri
) : AzOptions
{
    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}