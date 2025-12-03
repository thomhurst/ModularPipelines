using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "activity-log", "alert", "action-group", "add")]
public record AzMonitorActivityLogAlertActionGroupAddOptions(
[property: CliOption("--action-group")] string ActionGroup
) : AzOptions
{
    [CliOption("--activity-log-alert-name")]
    public string? ActivityLogAlertName { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliFlag("--reset")]
    public bool? Reset { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--strict")]
    public bool? Strict { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--webhook-properties")]
    public string? WebhookProperties { get; set; }
}