using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-snapshot-schedule")]
public record AwsRedshiftCreateSnapshotScheduleOptions : AwsOptions
{
    [CommandSwitch("--schedule-definitions")]
    public string[]? ScheduleDefinitions { get; set; }

    [CommandSwitch("--schedule-identifier")]
    public string? ScheduleIdentifier { get; set; }

    [CommandSwitch("--schedule-description")]
    public string? ScheduleDescription { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--next-invocations")]
    public int? NextInvocations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}