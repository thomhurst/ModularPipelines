using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("new-relic", "monitor", "tag-rule", "update")]
public record AzNewRelicMonitorTagRuleUpdateOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--log-rules")]
    public string? LogRules { get; set; }

    [CliOption("--metric-rules")]
    public string? MetricRules { get; set; }

    [CliOption("--monitor-name")]
    public string? MonitorName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}