using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "scheduled-query", "create")]
public record AzMonitorScheduledQueryCreateOptions(
[property: CliOption("--condition")] string Condition,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--scopes")] string Scopes
) : AzOptions
{
    [CliOption("--action-groups")]
    public string? ActionGroups { get; set; }

    [CliFlag("--auto-mitigate")]
    public bool? AutoMitigate { get; set; }

    [CliFlag("--check-ws-alerts-storage")]
    public bool? CheckWsAlertsStorage { get; set; }

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

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mad")]
    public string? Mad { get; set; }

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliFlag("--skip-query-validation")]
    public bool? SkipQueryValidation { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--target-resource-type")]
    public string? TargetResourceType { get; set; }

    [CliOption("--window-size")]
    public string? WindowSize { get; set; }
}