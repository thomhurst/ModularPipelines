using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-snapshot-schedule")]
public record AwsRedshiftModifySnapshotScheduleOptions(
[property: CliOption("--schedule-identifier")] string ScheduleIdentifier,
[property: CliOption("--schedule-definitions")] string[] ScheduleDefinitions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}