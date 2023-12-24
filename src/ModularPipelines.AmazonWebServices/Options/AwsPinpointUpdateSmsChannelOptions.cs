using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-sms-channel")]
public record AwsPinpointUpdateSmsChannelOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--sms-channel-request")] string SmsChannelRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}