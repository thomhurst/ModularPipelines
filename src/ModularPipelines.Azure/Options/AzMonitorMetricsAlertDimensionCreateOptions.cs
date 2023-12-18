using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "metrics", "alert", "dimension", "create")]
public record AzMonitorMetricsAlertDimensionCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--value")] string Value
) : AzOptions
{
    [CommandSwitch("--op")]
    public string? Op { get; set; }
}