using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-sms-channel")]
public record AwsPinpointUpdateSmsChannelOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--sms-channel-request")] string SmsChannelRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}