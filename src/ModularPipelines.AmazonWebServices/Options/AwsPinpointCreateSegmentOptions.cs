using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "create-segment")]
public record AwsPinpointCreateSegmentOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--write-segment-request")] string WriteSegmentRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}