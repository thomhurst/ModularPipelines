using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "scheduled-query", "create")]
public record AzMonitorScheduledQueryCreateOptions(
[property: CommandSwitch("--condition")] string Condition,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--scopes")] string Scopes
) : AzOptions
{
    [CommandSwitch("--action-groups")]
    public string? ActionGroups { get; set; }

    [BooleanCommandSwitch("--auto-mitigate")]
    public bool? AutoMitigate { get; set; }

    [BooleanCommandSwitch("--check-ws-alerts-storage")]
    public bool? CheckWsAlertsStorage { get; set; }

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

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mad")]
    public string? Mad { get; set; }

    [CommandSwitch("--severity")]
    public string? Severity { get; set; }

    [BooleanCommandSwitch("--skip-query-validation")]
    public bool? SkipQueryValidation { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--target-resource-type")]
    public string? TargetResourceType { get; set; }

    [CommandSwitch("--window-size")]
    public string? WindowSize { get; set; }
}

