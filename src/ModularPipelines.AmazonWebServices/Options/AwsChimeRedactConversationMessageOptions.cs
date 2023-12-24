using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "redact-conversation-message")]
public record AwsChimeRedactConversationMessageOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--conversation-id")] string ConversationId,
[property: CommandSwitch("--message-id")] string MessageId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}