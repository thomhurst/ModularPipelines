using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wisdom", "notify-recommendations-received")]
public record AwsWisdomNotifyRecommendationsReceivedOptions(
[property: CliOption("--assistant-id")] string AssistantId,
[property: CliOption("--recommendation-ids")] string[] RecommendationIds,
[property: CliOption("--session-id")] string SessionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}