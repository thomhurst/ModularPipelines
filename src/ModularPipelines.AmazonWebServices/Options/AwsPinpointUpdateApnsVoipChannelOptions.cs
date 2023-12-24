using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-apns-voip-channel")]
public record AwsPinpointUpdateApnsVoipChannelOptions(
[property: CommandSwitch("--apns-voip-channel-request")] string ApnsVoipChannelRequest,
[property: CommandSwitch("--application-id")] string ApplicationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}