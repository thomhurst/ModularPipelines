using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "list-anomalies")]
public record AwsLogsListAnomaliesOptions : AwsOptions
{
    [CliOption("--anomaly-detector-arn")]
    public string? AnomalyDetectorArn { get; set; }

    [CliOption("--suppression-state")]
    public string? SuppressionState { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}