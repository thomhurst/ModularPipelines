using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "send-users-messages")]
public record AwsPinpointSendUsersMessagesOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--send-users-message-request")] string SendUsersMessageRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}