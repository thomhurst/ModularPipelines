using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "scheduled-query", "update")]
public record AzMonitorScheduledQueryUpdateOptions : AzOptions
{
    [CliOption("--action-groups")]
    public string? ActionGroups { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--auto-mitigate")]
    public bool? AutoMitigate { get; set; }

    [CliFlag("--check-ws-alerts-storage")]
    public bool? CheckWsAlertsStorage { get; set; }

    [CliOption("--condition")]
    public string? Condition { get; set; }

    [CliOption("--condition-query")]
    public string? ConditionQuery { get; set; }

    [CliOption("--custom-properties")]
    public string? CustomProperties { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--evaluation-frequency")]
    public string? EvaluationFrequency { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--mad")]
    public string? Mad { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliFlag("--skip-query-validation")]
    public bool? SkipQueryValidation { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--target-resource-type")]
    public string? TargetResourceType { get; set; }

    [CliOption("--window-size")]
    public string? WindowSize { get; set; }
}