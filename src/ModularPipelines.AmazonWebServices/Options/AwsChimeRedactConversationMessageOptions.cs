using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "redact-conversation-message")]
public record AwsChimeRedactConversationMessageOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--conversation-id")] string ConversationId,
[property: CliOption("--message-id")] string MessageId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}