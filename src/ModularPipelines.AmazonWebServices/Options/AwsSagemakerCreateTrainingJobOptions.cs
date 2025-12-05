using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-training-job")]
public record AwsSagemakerCreateTrainingJobOptions(
[property: CliOption("--training-job-name")] string TrainingJobName,
[property: CliOption("--algorithm-specification")] string AlgorithmSpecification,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--output-data-config")] string OutputDataConfig,
[property: CliOption("--resource-config")] string ResourceConfig,
[property: CliOption("--stopping-condition")] string StoppingCondition
) : AwsOptions
{
    [CliOption("--hyper-parameters")]
    public IEnumerable<KeyValue>? HyperParameters { get; set; }

    [CliOption("--input-data-config")]
    public string[]? InputDataConfig { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--checkpoint-config")]
    public string? CheckpointConfig { get; set; }

    [CliOption("--debug-hook-config")]
    public string? DebugHookConfig { get; set; }

    [CliOption("--debug-rule-configurations")]
    public string[]? DebugRuleConfigurations { get; set; }

    [CliOption("--tensor-board-output-config")]
    public string? TensorBoardOutputConfig { get; set; }

    [CliOption("--experiment-config")]
    public string? ExperimentConfig { get; set; }

    [CliOption("--profiler-config")]
    public string? ProfilerConfig { get; set; }

    [CliOption("--profiler-rule-configurations")]
    public string[]? ProfilerRuleConfigurations { get; set; }

    [CliOption("--environment")]
    public IEnumerable<KeyValue>? Environment { get; set; }

    [CliOption("--retry-strategy")]
    public string? RetryStrategy { get; set; }

    [CliOption("--remote-debug-config")]
    public string? RemoteDebugConfig { get; set; }

    [CliOption("--infra-check-config")]
    public string? InfraCheckConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}