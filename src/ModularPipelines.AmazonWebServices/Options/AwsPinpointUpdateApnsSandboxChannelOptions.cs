using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-apns-sandbox-channel")]
public record AwsPinpointUpdateApnsSandboxChannelOptions(
[property: CommandSwitch("--apns-sandbox-channel-request")] string ApnsSandboxChannelRequest,
[property: CommandSwitch("--application-id")] string ApplicationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}