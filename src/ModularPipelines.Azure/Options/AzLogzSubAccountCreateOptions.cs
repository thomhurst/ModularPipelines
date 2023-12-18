using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logz", "sub-account", "create")]
public record AzLogzSubAccountCreateOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--monitoring-status")]
    public string? MonitoringStatus { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--org-properties")]
    public string? OrgProperties { get; set; }

    [CommandSwitch("--plan-data")]
    public string? PlanData { get; set; }

    [CommandSwitch("--subscription-status")]
    public string? SubscriptionStatus { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--user-info")]
    public string? UserInfo { get; set; }
}