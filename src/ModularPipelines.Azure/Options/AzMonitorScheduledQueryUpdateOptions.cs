using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "scheduled-query", "update")]
public record AzMonitorScheduledQueryUpdateOptions : AzOptions
{
    [CommandSwitch("--action-groups")]
    public string? ActionGroups { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--auto-mitigate")]
    public bool? AutoMitigate { get; set; }

    [BooleanCommandSwitch("--check-ws-alerts-storage")]
    public bool? CheckWsAlertsStorage { get; set; }

    [CommandSwitch("--condition")]
    public string? Condition { get; set; }

    [CommandSwitch("--condition-query")]
    public string? ConditionQuery { get; set; }

    [CommandSwitch("--custom-properties")]
    public string? CustomProperties { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--evaluation-frequency")]
    public string? EvaluationFrequency { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--mad")]
    public string? Mad { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--severity")]
    public string? Severity { get; set; }

    [BooleanCommandSwitch("--skip-query-validation")]
    public bool? SkipQueryValidation { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--target-resource-type")]
    public string? TargetResourceType { get; set; }

    [CommandSwitch("--window-size")]
    public string? WindowSize { get; set; }
}