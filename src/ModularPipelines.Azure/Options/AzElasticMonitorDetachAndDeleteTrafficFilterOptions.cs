using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic", "monitor", "detach-and-delete-traffic-filter")]
public record AzElasticMonitorDetachAndDeleteTrafficFilterOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--monitor-name")]
    public string? MonitorName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--ruleset-id")]
    public string? RulesetId { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}