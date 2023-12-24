using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect-contact-lens", "list-realtime-contact-analysis-segments")]
public record AwsConnectContactLensListRealtimeContactAnalysisSegmentsOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--contact-id")] string ContactId
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}