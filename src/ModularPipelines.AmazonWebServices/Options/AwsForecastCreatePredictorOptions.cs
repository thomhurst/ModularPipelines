using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "create-predictor")]
public record AwsForecastCreatePredictorOptions(
[property: CommandSwitch("--predictor-name")] string PredictorName,
[property: CommandSwitch("--forecast-horizon")] int ForecastHorizon,
[property: CommandSwitch("--input-data-config")] string InputDataConfig,
[property: CommandSwitch("--featurization-config")] string FeaturizationConfig
) : AwsOptions
{
    [CommandSwitch("--algorithm-arn")]
    public string? AlgorithmArn { get; set; }

    [CommandSwitch("--forecast-types")]
    public string[]? ForecastTypes { get; set; }

    [CommandSwitch("--auto-ml-override-strategy")]
    public string? AutoMlOverrideStrategy { get; set; }

    [CommandSwitch("--training-parameters")]
    public IEnumerable<KeyValue>? TrainingParameters { get; set; }

    [CommandSwitch("--evaluation-parameters")]
    public string? EvaluationParameters { get; set; }

    [CommandSwitch("--hpo-config")]
    public string? HpoConfig { get; set; }

    [CommandSwitch("--encryption-config")]
    public string? EncryptionConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--optimization-metric")]
    public string? OptimizationMetric { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}