using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "send-otp-message")]
public record AwsPinpointSendOtpMessageOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--send-otp-message-request-parameters")] string SendOtpMessageRequestParameters
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}