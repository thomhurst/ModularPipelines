using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-apns-voip-sandbox-channel")]
public record AwsPinpointUpdateApnsVoipSandboxChannelOptions(
[property: CommandSwitch("--apns-voip-sandbox-channel-request")] string ApnsVoipSandboxChannelRequest,
[property: CommandSwitch("--application-id")] string ApplicationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}