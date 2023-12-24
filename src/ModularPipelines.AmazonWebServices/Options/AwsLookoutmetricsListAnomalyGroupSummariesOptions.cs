using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "list-anomaly-group-summaries")]
public record AwsLookoutmetricsListAnomalyGroupSummariesOptions(
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn,
[property: CommandSwitch("--sensitivity-threshold")] int SensitivityThreshold
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}