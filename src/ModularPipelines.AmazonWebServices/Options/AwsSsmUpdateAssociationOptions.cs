using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-association")]
public record AwsSsmUpdateAssociationOptions(
[property: CliOption("--association-id")] string AssociationId
) : AwsOptions
{
    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--document-version")]
    public string? DocumentVersion { get; set; }

    [CliOption("--schedule-expression")]
    public string? ScheduleExpression { get; set; }

    [CliOption("--output-location")]
    public string? OutputLocation { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--targets")]
    public string[]? Targets { get; set; }

    [CliOption("--association-name")]
    public string? AssociationName { get; set; }

    [CliOption("--association-version")]
    public string? AssociationVersion { get; set; }

    [CliOption("--automation-target-parameter-name")]
    public string? AutomationTargetParameterName { get; set; }

    [CliOption("--max-errors")]
    public string? MaxErrors { get; set; }

    [CliOption("--max-concurrency")]
    public string? MaxConcurrency { get; set; }

    [CliOption("--compliance-severity")]
    public string? ComplianceSeverity { get; set; }

    [CliOption("--sync-compliance")]
    public string? SyncCompliance { get; set; }

    [CliOption("--calendar-names")]
    public string[]? CalendarNames { get; set; }

    [CliOption("--target-locations")]
    public string[]? TargetLocations { get; set; }

    [CliOption("--schedule-offset")]
    public int? ScheduleOffset { get; set; }

    [CliOption("--target-maps")]
    public string[]? TargetMaps { get; set; }

    [CliOption("--alarm-configuration")]
    public string? AlarmConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}