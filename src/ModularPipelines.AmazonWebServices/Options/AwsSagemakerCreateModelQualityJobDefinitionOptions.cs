using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-model-quality-job-definition")]
public record AwsSagemakerCreateModelQualityJobDefinitionOptions(
[property: CliOption("--job-definition-name")] string JobDefinitionName,
[property: CliOption("--model-quality-app-specification")] string ModelQualityAppSpecification,
[property: CliOption("--model-quality-job-input")] string ModelQualityJobInput,
[property: CliOption("--model-quality-job-output-config")] string ModelQualityJobOutputConfig,
[property: CliOption("--job-resources")] string JobResources,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--model-quality-baseline-config")]
    public string? ModelQualityBaselineConfig { get; set; }

    [CliOption("--network-config")]
    public string? NetworkConfig { get; set; }

    [CliOption("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}