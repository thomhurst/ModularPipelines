using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastic", "monitor", "detach-traffic-filter")]
public record AzElasticMonitorDetachTrafficFilterOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--monitor-name")]
    public string? MonitorName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--ruleset-id")]
    public string? RulesetId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}