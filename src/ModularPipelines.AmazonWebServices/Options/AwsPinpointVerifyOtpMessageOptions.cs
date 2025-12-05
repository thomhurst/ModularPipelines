using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "verify-otp-message")]
public record AwsPinpointVerifyOtpMessageOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--verify-otp-message-request-parameters")] string VerifyOtpMessageRequestParameters
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}