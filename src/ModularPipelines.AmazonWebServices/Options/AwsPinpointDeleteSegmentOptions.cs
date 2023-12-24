using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "delete-segment")]
public record AwsPinpointDeleteSegmentOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--segment-id")] string SegmentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}