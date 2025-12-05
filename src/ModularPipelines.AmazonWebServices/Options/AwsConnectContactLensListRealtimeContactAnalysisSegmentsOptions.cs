using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect-contact-lens", "list-realtime-contact-analysis-segments")]
public record AwsConnectContactLensListRealtimeContactAnalysisSegmentsOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-id")] string ContactId
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}