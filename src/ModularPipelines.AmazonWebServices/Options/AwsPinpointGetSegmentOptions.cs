using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "get-segment")]
public record AwsPinpointGetSegmentOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--segment-id")] string SegmentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}