using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "send-otp-message")]
public record AwsPinpointSendOtpMessageOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--send-otp-message-request-parameters")] string SendOtpMessageRequestParameters
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}