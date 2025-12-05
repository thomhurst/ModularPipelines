using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-model-explainability-job-definition")]
public record AwsSagemakerCreateModelExplainabilityJobDefinitionOptions(
[property: CliOption("--job-definition-name")] string JobDefinitionName,
[property: CliOption("--model-explainability-app-specification")] string ModelExplainabilityAppSpecification,
[property: CliOption("--model-explainability-job-input")] string ModelExplainabilityJobInput,
[property: CliOption("--model-explainability-job-output-config")] string ModelExplainabilityJobOutputConfig,
[property: CliOption("--job-resources")] string JobResources,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--model-explainability-baseline-config")]
    public string? ModelExplainabilityBaselineConfig { get; set; }

    [CliOption("--network-config")]
    public string? NetworkConfig { get; set; }

    [CliOption("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}