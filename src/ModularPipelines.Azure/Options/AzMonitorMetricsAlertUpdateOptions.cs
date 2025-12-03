using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "metrics", "alert", "update")]
public record AzMonitorMetricsAlertUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--add-action")]
    public string? AddAction { get; set; }

    [CliOption("--add-condition")]
    public string? AddCondition { get; set; }

    [CliFlag("--auto-mitigate")]
    public bool? AutoMitigate { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--evaluation-frequency")]
    public string? EvaluationFrequency { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--remove-actions")]
    public string? RemoveActions { get; set; }

    [CliOption("--remove-conditions")]
    public string? RemoveConditions { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scopes")]
    public string? Scopes { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--window-size")]
    public string? WindowSize { get; set; }
}