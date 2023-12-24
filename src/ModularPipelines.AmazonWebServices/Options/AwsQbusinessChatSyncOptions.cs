using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "chat-sync")]
public record AwsQbusinessChatSyncOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--action-execution")]
    public string? ActionExecution { get; set; }

    [CommandSwitch("--attachments")]
    public string[]? Attachments { get; set; }

    [CommandSwitch("--attribute-filter")]
    public string? AttributeFilter { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--conversation-id")]
    public string? ConversationId { get; set; }

    [CommandSwitch("--parent-message-id")]
    public string? ParentMessageId { get; set; }

    [CommandSwitch("--user-groups")]
    public string[]? UserGroups { get; set; }

    [CommandSwitch("--user-message")]
    public string? UserMessage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}