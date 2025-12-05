using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-auto-predictor")]
public record AwsForecastCreateAutoPredictorOptions(
[property: CliOption("--predictor-name")] string PredictorName
) : AwsOptions
{
    [CliOption("--forecast-horizon")]
    public int? ForecastHorizon { get; set; }

    [CliOption("--forecast-types")]
    public string[]? ForecastTypes { get; set; }

    [CliOption("--forecast-dimensions")]
    public string[]? ForecastDimensions { get; set; }

    [CliOption("--forecast-frequency")]
    public string? ForecastFrequency { get; set; }

    [CliOption("--data-config")]
    public string? DataConfig { get; set; }

    [CliOption("--encryption-config")]
    public string? EncryptionConfig { get; set; }

    [CliOption("--reference-predictor-arn")]
    public string? ReferencePredictorArn { get; set; }

    [CliOption("--optimization-metric")]
    public string? OptimizationMetric { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--monitor-config")]
    public string? MonitorConfig { get; set; }

    [CliOption("--time-alignment-boundary")]
    public string? TimeAlignmentBoundary { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}