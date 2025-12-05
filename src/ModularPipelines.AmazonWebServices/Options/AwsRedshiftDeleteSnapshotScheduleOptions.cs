using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "delete-snapshot-schedule")]
public record AwsRedshiftDeleteSnapshotScheduleOptions(
[property: CliOption("--schedule-identifier")] string ScheduleIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}