using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-anomalies")]
public record AwsCeGetAnomaliesOptions(
[property: CliOption("--date-interval")] string DateInterval
) : AwsOptions
{
    [CliOption("--monitor-arn")]
    public string? MonitorArn { get; set; }

    [CliOption("--feedback")]
    public string? Feedback { get; set; }

    [CliOption("--total-impact")]
    public string? TotalImpact { get; set; }

    [CliOption("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}