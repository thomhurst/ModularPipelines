using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-hyper-parameter-tuning-job")]
public record AwsSagemakerCreateHyperParameterTuningJobOptions(
[property: CommandSwitch("--hyper-parameter-tuning-job-name")] string HyperParameterTuningJobName,
[property: CommandSwitch("--hyper-parameter-tuning-job-config")] string HyperParameterTuningJobConfig
) : AwsOptions
{
    [CommandSwitch("--training-job-definition")]
    public string? TrainingJobDefinition { get; set; }

    [CommandSwitch("--training-job-definitions")]
    public string[]? TrainingJobDefinitions { get; set; }

    [CommandSwitch("--warm-start-config")]
    public string? WarmStartConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--autotune")]
    public string? Autotune { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}