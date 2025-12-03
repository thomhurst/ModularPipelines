using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-processing-job")]
public record AwsSagemakerCreateProcessingJobOptions(
[property: CliOption("--processing-job-name")] string ProcessingJobName,
[property: CliOption("--processing-resources")] string ProcessingResources,
[property: CliOption("--app-specification")] string AppSpecification,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--processing-inputs")]
    public string[]? ProcessingInputs { get; set; }

    [CliOption("--processing-output-config")]
    public string? ProcessingOutputConfig { get; set; }

    [CliOption("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CliOption("--environment")]
    public IEnumerable<KeyValue>? Environment { get; set; }

    [CliOption("--network-config")]
    public string? NetworkConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--experiment-config")]
    public string? ExperimentConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}