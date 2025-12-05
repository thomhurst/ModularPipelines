using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-apns-voip-sandbox-channel")]
public record AwsPinpointUpdateApnsVoipSandboxChannelOptions(
[property: CliOption("--apns-voip-sandbox-channel-request")] string ApnsVoipSandboxChannelRequest,
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}