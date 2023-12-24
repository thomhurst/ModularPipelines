using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "create-auto-predictor")]
public record AwsForecastCreateAutoPredictorOptions(
[property: CommandSwitch("--predictor-name")] string PredictorName
) : AwsOptions
{
    [CommandSwitch("--forecast-horizon")]
    public int? ForecastHorizon { get; set; }

    [CommandSwitch("--forecast-types")]
    public string[]? ForecastTypes { get; set; }

    [CommandSwitch("--forecast-dimensions")]
    public string[]? ForecastDimensions { get; set; }

    [CommandSwitch("--forecast-frequency")]
    public string? ForecastFrequency { get; set; }

    [CommandSwitch("--data-config")]
    public string? DataConfig { get; set; }

    [CommandSwitch("--encryption-config")]
    public string? EncryptionConfig { get; set; }

    [CommandSwitch("--reference-predictor-arn")]
    public string? ReferencePredictorArn { get; set; }

    [CommandSwitch("--optimization-metric")]
    public string? OptimizationMetric { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--monitor-config")]
    public string? MonitorConfig { get; set; }

    [CommandSwitch("--time-alignment-boundary")]
    public string? TimeAlignmentBoundary { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}