using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-training-job")]
public record AwsSagemakerUpdateTrainingJobOptions(
[property: CliOption("--training-job-name")] string TrainingJobName
) : AwsOptions
{
    [CliOption("--profiler-config")]
    public string? ProfilerConfig { get; set; }

    [CliOption("--profiler-rule-configurations")]
    public string[]? ProfilerRuleConfigurations { get; set; }

    [CliOption("--resource-config")]
    public string? ResourceConfig { get; set; }

    [CliOption("--remote-debug-config")]
    public string? RemoteDebugConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}