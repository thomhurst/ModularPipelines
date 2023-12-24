using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "create-association")]
public record AwsSsmCreateAssociationOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--document-version")]
    public string? DocumentVersion { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--targets")]
    public string[]? Targets { get; set; }

    [CommandSwitch("--schedule-expression")]
    public string? ScheduleExpression { get; set; }

    [CommandSwitch("--output-location")]
    public string? OutputLocation { get; set; }

    [CommandSwitch("--association-name")]
    public string? AssociationName { get; set; }

    [CommandSwitch("--automation-target-parameter-name")]
    public string? AutomationTargetParameterName { get; set; }

    [CommandSwitch("--max-errors")]
    public string? MaxErrors { get; set; }

    [CommandSwitch("--max-concurrency")]
    public string? MaxConcurrency { get; set; }

    [CommandSwitch("--compliance-severity")]
    public string? ComplianceSeverity { get; set; }

    [CommandSwitch("--sync-compliance")]
    public string? SyncCompliance { get; set; }

    [CommandSwitch("--calendar-names")]
    public string[]? CalendarNames { get; set; }

    [CommandSwitch("--target-locations")]
    public string[]? TargetLocations { get; set; }

    [CommandSwitch("--schedule-offset")]
    public int? ScheduleOffset { get; set; }

    [CommandSwitch("--target-maps")]
    public string[]? TargetMaps { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--alarm-configuration")]
    public string? AlarmConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}