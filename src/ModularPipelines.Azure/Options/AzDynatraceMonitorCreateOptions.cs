using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynatrace", "monitor", "create")]
public record AzDynatraceMonitorCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--monitoring-status")]
    public string? MonitoringStatus { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--plan-data")]
    public string? PlanData { get; set; }

    [CliOption("--subscription-status")]
    public string? SubscriptionStatus { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-info")]
    public string? UserInfo { get; set; }
}