using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "metrics", "list-namespaces")]
public record AzMonitorMetricsListNamespacesOptions(
[property: CliOption("--resource-uri")] string ResourceUri
) : AzOptions
{
    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}