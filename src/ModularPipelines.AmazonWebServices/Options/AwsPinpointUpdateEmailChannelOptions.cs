using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-email-channel")]
public record AwsPinpointUpdateEmailChannelOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--email-channel-request")] string EmailChannelRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}