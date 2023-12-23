using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-training-job")]
public record AwsSagemakerCreateTrainingJobOptions(
[property: CommandSwitch("--training-job-name")] string TrainingJobName,
[property: CommandSwitch("--algorithm-specification")] string AlgorithmSpecification,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--output-data-config")] string OutputDataConfig,
[property: CommandSwitch("--resource-config")] string ResourceConfig,
[property: CommandSwitch("--stopping-condition")] string StoppingCondition
) : AwsOptions
{
    [CommandSwitch("--hyper-parameters")]
    public IEnumerable<KeyValue>? HyperParameters { get; set; }

    [CommandSwitch("--input-data-config")]
    public string[]? InputDataConfig { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--checkpoint-config")]
    public string? CheckpointConfig { get; set; }

    [CommandSwitch("--debug-hook-config")]
    public string? DebugHookConfig { get; set; }

    [CommandSwitch("--debug-rule-configurations")]
    public string[]? DebugRuleConfigurations { get; set; }

    [CommandSwitch("--tensor-board-output-config")]
    public string? TensorBoardOutputConfig { get; set; }

    [CommandSwitch("--experiment-config")]
    public string? ExperimentConfig { get; set; }

    [CommandSwitch("--profiler-config")]
    public string? ProfilerConfig { get; set; }

    [CommandSwitch("--profiler-rule-configurations")]
    public string[]? ProfilerRuleConfigurations { get; set; }

    [CommandSwitch("--environment")]
    public IEnumerable<KeyValue>? Environment { get; set; }

    [CommandSwitch("--retry-strategy")]
    public string? RetryStrategy { get; set; }

    [CommandSwitch("--remote-debug-config")]
    public string? RemoteDebugConfig { get; set; }

    [CommandSwitch("--infra-check-config")]
    public string? InfraCheckConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}