using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-snapshot-schedule")]
public record AwsRedshiftCreateSnapshotScheduleOptions : AwsOptions
{
    [CliOption("--schedule-definitions")]
    public string[]? ScheduleDefinitions { get; set; }

    [CliOption("--schedule-identifier")]
    public string? ScheduleIdentifier { get; set; }

    [CliOption("--schedule-description")]
    public string? ScheduleDescription { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--next-invocations")]
    public int? NextInvocations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}