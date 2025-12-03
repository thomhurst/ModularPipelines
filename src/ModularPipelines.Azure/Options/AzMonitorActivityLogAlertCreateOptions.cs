using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "activity-log", "alert", "create")]
public record AzMonitorActivityLogAlertCreateOptions(
[property: CliOption("--activity-log-alert-name")] string ActivityLogAlertName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--action-group")]
    public string? ActionGroup { get; set; }

    [CliOption("--all-of")]
    public string? AllOf { get; set; }

    [CliOption("--condition")]
    public string? Condition { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disable")]
    public bool? Disable { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--webhook-properties")]
    public string? WebhookProperties { get; set; }
}