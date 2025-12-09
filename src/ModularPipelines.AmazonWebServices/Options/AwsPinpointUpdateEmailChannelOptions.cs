using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-email-channel")]
public record AwsPinpointUpdateEmailChannelOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--email-channel-request")] string EmailChannelRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}