using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qconnect", "get-recommendations")]
public record AwsQconnectGetRecommendationsOptions(
[property: CliOption("--assistant-id")] string AssistantId,
[property: CliOption("--session-id")] string SessionId
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--wait-time-seconds")]
    public int? WaitTimeSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}