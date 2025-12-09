using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-predictor")]
public record AwsForecastCreatePredictorOptions(
[property: CliOption("--predictor-name")] string PredictorName,
[property: CliOption("--forecast-horizon")] int ForecastHorizon,
[property: CliOption("--input-data-config")] string InputDataConfig,
[property: CliOption("--featurization-config")] string FeaturizationConfig
) : AwsOptions
{
    [CliOption("--algorithm-arn")]
    public string? AlgorithmArn { get; set; }

    [CliOption("--forecast-types")]
    public string[]? ForecastTypes { get; set; }

    [CliOption("--auto-ml-override-strategy")]
    public string? AutoMlOverrideStrategy { get; set; }

    [CliOption("--training-parameters")]
    public IEnumerable<KeyValue>? TrainingParameters { get; set; }

    [CliOption("--evaluation-parameters")]
    public string? EvaluationParameters { get; set; }

    [CliOption("--hpo-config")]
    public string? HpoConfig { get; set; }

    [CliOption("--encryption-config")]
    public string? EncryptionConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--optimization-metric")]
    public string? OptimizationMetric { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}