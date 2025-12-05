using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-hyper-parameter-tuning-job")]
public record AwsSagemakerCreateHyperParameterTuningJobOptions(
[property: CliOption("--hyper-parameter-tuning-job-name")] string HyperParameterTuningJobName,
[property: CliOption("--hyper-parameter-tuning-job-config")] string HyperParameterTuningJobConfig
) : AwsOptions
{
    [CliOption("--training-job-definition")]
    public string? TrainingJobDefinition { get; set; }

    [CliOption("--training-job-definitions")]
    public string[]? TrainingJobDefinitions { get; set; }

    [CliOption("--warm-start-config")]
    public string? WarmStartConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--autotune")]
    public string? Autotune { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}