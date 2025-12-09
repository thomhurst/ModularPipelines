using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "delete-conversation")]
public record AwsQbusinessDeleteConversationOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--conversation-id")] string ConversationId,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}