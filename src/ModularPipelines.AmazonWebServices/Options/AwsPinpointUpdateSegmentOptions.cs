using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-segment")]
public record AwsPinpointUpdateSegmentOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--segment-id")] string SegmentId,
[property: CommandSwitch("--write-segment-request")] string WriteSegmentRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}