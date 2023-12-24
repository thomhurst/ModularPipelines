using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "get-segment-version")]
public record AwsPinpointGetSegmentVersionOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--segment-id")] string SegmentId,
[property: CommandSwitch("--segment-version")] string SegmentVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}