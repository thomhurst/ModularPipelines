using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-training-job")]
public record AwsSagemakerUpdateTrainingJobOptions(
[property: CommandSwitch("--training-job-name")] string TrainingJobName
) : AwsOptions
{
    [CommandSwitch("--profiler-config")]
    public string? ProfilerConfig { get; set; }

    [CommandSwitch("--profiler-rule-configurations")]
    public string[]? ProfilerRuleConfigurations { get; set; }

    [CommandSwitch("--resource-config")]
    public string? ResourceConfig { get; set; }

    [CommandSwitch("--remote-debug-config")]
    public string? RemoteDebugConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}