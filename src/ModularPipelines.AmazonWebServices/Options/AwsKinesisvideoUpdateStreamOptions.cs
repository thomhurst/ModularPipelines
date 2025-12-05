using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisvideo", "update-stream")]
public record AwsKinesisvideoUpdateStreamOptions(
[property: CliOption("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--device-name")]
    public string? DeviceName { get; set; }

    [CliOption("--media-type")]
    public string? MediaType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}