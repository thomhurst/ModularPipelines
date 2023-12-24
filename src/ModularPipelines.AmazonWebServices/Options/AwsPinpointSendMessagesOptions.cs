using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "send-messages")]
public record AwsPinpointSendMessagesOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--message-request")] string MessageRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}