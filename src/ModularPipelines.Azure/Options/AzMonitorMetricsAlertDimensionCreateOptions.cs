using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "metrics", "alert", "dimension", "create")]
public record AzMonitorMetricsAlertDimensionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--value")] string Value
) : AzOptions
{
    [CliOption("--op")]
    public string? Op { get; set; }
}