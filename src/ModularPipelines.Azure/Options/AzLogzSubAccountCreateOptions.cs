using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logz", "sub-account", "create")]
public record AzLogzSubAccountCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--monitoring-status")]
    public string? MonitoringStatus { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--org-properties")]
    public string? OrgProperties { get; set; }

    [CliOption("--plan-data")]
    public string? PlanData { get; set; }

    [CliOption("--subscription-status")]
    public string? SubscriptionStatus { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--user-info")]
    public string? UserInfo { get; set; }
}