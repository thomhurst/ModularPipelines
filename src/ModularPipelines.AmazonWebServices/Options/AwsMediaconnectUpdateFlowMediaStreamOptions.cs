using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "update-flow-media-stream")]
public record AwsMediaconnectUpdateFlowMediaStreamOptions(
[property: CommandSwitch("--flow-arn")] string FlowArn,
[property: CommandSwitch("--media-stream-name")] string MediaStreamName
) : AwsOptions
{
    [CommandSwitch("--attributes")]
    public string? Attributes { get; set; }

    [CommandSwitch("--clock-rate")]
    public int? ClockRate { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--media-stream-type")]
    public string? MediaStreamType { get; set; }

    [CommandSwitch("--video-format")]
    public string? VideoFormat { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}