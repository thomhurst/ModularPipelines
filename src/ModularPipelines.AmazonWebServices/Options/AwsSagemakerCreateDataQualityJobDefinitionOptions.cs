using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-data-quality-job-definition")]
public record AwsSagemakerCreateDataQualityJobDefinitionOptions(
[property: CliOption("--job-definition-name")] string JobDefinitionName,
[property: CliOption("--data-quality-app-specification")] string DataQualityAppSpecification,
[property: CliOption("--data-quality-job-input")] string DataQualityJobInput,
[property: CliOption("--data-quality-job-output-config")] string DataQualityJobOutputConfig,
[property: CliOption("--job-resources")] string JobResources,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--data-quality-baseline-config")]
    public string? DataQualityBaselineConfig { get; set; }

    [CliOption("--network-config")]
    public string? NetworkConfig { get; set; }

    [CliOption("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}