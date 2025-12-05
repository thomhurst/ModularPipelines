using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisvideo", "delete-stream")]
public record AwsKinesisvideoDeleteStreamOptions(
[property: CliOption("--stream-arn")] string StreamArn
) : AwsOptions
{
    [CliOption("--current-version")]
    public string? CurrentVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}