using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "activity-log", "alert", "update")]
public record AzMonitorActivityLogAlertUpdateOptions : AzOptions
{
    [CliOption("--activity-log-alert-name")]
    public string? ActivityLogAlertName { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--all-of")]
    public string? AllOf { get; set; }

    [CliOption("--condition")]
    public string? Condition { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}