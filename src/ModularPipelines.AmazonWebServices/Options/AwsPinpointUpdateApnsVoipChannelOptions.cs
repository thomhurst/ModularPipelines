using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-apns-voip-channel")]
public record AwsPinpointUpdateApnsVoipChannelOptions(
[property: CliOption("--apns-voip-channel-request")] string ApnsVoipChannelRequest,
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}