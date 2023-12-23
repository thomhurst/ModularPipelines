using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qconnect", "notify-recommendations-received")]
public record AwsQconnectNotifyRecommendationsReceivedOptions(
[property: CommandSwitch("--assistant-id")] string AssistantId,
[property: CommandSwitch("--recommendation-ids")] string[] RecommendationIds,
[property: CommandSwitch("--session-id")] string SessionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}