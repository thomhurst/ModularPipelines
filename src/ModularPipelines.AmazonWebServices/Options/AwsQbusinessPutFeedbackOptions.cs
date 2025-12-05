using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "put-feedback")]
public record AwsQbusinessPutFeedbackOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--conversation-id")] string ConversationId,
[property: CliOption("--message-id")] string MessageId,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--message-copied-at")]
    public long? MessageCopiedAt { get; set; }

    [CliOption("--message-usefulness")]
    public string? MessageUsefulness { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}