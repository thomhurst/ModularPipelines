using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "chat-sync")]
public record AwsQbusinessChatSyncOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--action-execution")]
    public string? ActionExecution { get; set; }

    [CliOption("--attachments")]
    public string[]? Attachments { get; set; }

    [CliOption("--attribute-filter")]
    public string? AttributeFilter { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--conversation-id")]
    public string? ConversationId { get; set; }

    [CliOption("--parent-message-id")]
    public string? ParentMessageId { get; set; }

    [CliOption("--user-groups")]
    public string[]? UserGroups { get; set; }

    [CliOption("--user-message")]
    public string? UserMessage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}