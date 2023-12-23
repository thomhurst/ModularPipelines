using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "get-anomalies")]
public record AwsCeGetAnomaliesOptions(
[property: CommandSwitch("--date-interval")] string DateInterval
) : AwsOptions
{
    [CommandSwitch("--monitor-arn")]
    public string? MonitorArn { get; set; }

    [CommandSwitch("--feedback")]
    public string? Feedback { get; set; }

    [CommandSwitch("--total-impact")]
    public string? TotalImpact { get; set; }

    [CommandSwitch("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}