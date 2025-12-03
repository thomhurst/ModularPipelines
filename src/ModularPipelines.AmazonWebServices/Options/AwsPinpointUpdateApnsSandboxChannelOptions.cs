using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-apns-sandbox-channel")]
public record AwsPinpointUpdateApnsSandboxChannelOptions(
[property: CliOption("--apns-sandbox-channel-request")] string ApnsSandboxChannelRequest,
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}