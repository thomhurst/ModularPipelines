using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dynatrace", "monitor", "tag-rule", "create")]
public record AzDynatraceMonitorTagRuleCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--log-rules")]
    public string? LogRules { get; set; }

    [CliOption("--metric-rules")]
    public string? MetricRules { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}