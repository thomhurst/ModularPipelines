using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-snapshot-schedule")]
public record AwsStoragegatewayUpdateSnapshotScheduleOptions(
[property: CliOption("--volume-arn")] string VolumeArn,
[property: CliOption("--start-at")] int StartAt,
[property: CliOption("--recurrence-in-hours")] int RecurrenceInHours
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}