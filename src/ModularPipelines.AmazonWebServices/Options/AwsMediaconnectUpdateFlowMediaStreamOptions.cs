using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "update-flow-media-stream")]
public record AwsMediaconnectUpdateFlowMediaStreamOptions(
[property: CliOption("--flow-arn")] string FlowArn,
[property: CliOption("--media-stream-name")] string MediaStreamName
) : AwsOptions
{
    [CliOption("--attributes")]
    public string? Attributes { get; set; }

    [CliOption("--clock-rate")]
    public int? ClockRate { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--media-stream-type")]
    public string? MediaStreamType { get; set; }

    [CliOption("--video-format")]
    public string? VideoFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}