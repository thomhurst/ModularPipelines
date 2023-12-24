using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-processing-job")]
public record AwsSagemakerCreateProcessingJobOptions(
[property: CommandSwitch("--processing-job-name")] string ProcessingJobName,
[property: CommandSwitch("--processing-resources")] string ProcessingResources,
[property: CommandSwitch("--app-specification")] string AppSpecification,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--processing-inputs")]
    public string[]? ProcessingInputs { get; set; }

    [CommandSwitch("--processing-output-config")]
    public string? ProcessingOutputConfig { get; set; }

    [CommandSwitch("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CommandSwitch("--environment")]
    public IEnumerable<KeyValue>? Environment { get; set; }

    [CommandSwitch("--network-config")]
    public string? NetworkConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--experiment-config")]
    public string? ExperimentConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}