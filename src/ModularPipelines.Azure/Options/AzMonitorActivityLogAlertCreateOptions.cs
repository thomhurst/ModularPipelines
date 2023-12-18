using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "activity-log", "alert", "create")]
public record AzMonitorActivityLogAlertCreateOptions(
[property: CommandSwitch("--activity-log-alert-name")] string ActivityLogAlertName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--action-group")]
    public string? ActionGroup { get; set; }

    [CommandSwitch("--all-of")]
    public string? AllOf { get; set; }

    [CommandSwitch("--condition")]
    public string? Condition { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disable")]
    public bool? Disable { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--webhook-properties")]
    public string? WebhookProperties { get; set; }
}