using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisvideo", "update-data-retention")]
public record AwsKinesisvideoUpdateDataRetentionOptions(
[property: CliOption("--current-version")] string CurrentVersion,
[property: CliOption("--operation")] string Operation,
[property: CliOption("--data-retention-change-in-hours")] int DataRetentionChangeInHours
) : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}