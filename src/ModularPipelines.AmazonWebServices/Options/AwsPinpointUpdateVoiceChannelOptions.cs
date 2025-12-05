using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-voice-channel")]
public record AwsPinpointUpdateVoiceChannelOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--voice-channel-request")] string VoiceChannelRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}