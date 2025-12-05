using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis", "update-stream-mode")]
public record AwsKinesisUpdateStreamModeOptions(
[property: CliOption("--stream-arn")] string StreamArn,
[property: CliOption("--stream-mode-details")] string StreamModeDetails
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}