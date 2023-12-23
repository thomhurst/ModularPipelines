using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "verify-otp-message")]
public record AwsPinpointVerifyOtpMessageOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--verify-otp-message-request-parameters")] string VerifyOtpMessageRequestParameters
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}