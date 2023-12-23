using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-gcm-channel")]
public record AwsPinpointUpdateGcmChannelOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--gcm-channel-request")] string GcmChannelRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}