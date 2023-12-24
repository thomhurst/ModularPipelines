using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "put-feedback")]
public record AwsQbusinessPutFeedbackOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--conversation-id")] string ConversationId,
[property: CommandSwitch("--message-id")] string MessageId,
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--message-copied-at")]
    public long? MessageCopiedAt { get; set; }

    [CommandSwitch("--message-usefulness")]
    public string? MessageUsefulness { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}