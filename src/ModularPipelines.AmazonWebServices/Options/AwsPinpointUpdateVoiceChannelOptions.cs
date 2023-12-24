using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-voice-channel")]
public record AwsPinpointUpdateVoiceChannelOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--voice-channel-request")] string VoiceChannelRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}