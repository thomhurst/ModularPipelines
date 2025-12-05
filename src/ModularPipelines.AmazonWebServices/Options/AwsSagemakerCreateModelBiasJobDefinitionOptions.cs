using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-model-bias-job-definition")]
public record AwsSagemakerCreateModelBiasJobDefinitionOptions(
[property: CliOption("--job-definition-name")] string JobDefinitionName,
[property: CliOption("--model-bias-app-specification")] string ModelBiasAppSpecification,
[property: CliOption("--model-bias-job-input")] string ModelBiasJobInput,
[property: CliOption("--model-bias-job-output-config")] string ModelBiasJobOutputConfig,
[property: CliOption("--job-resources")] string JobResources,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--model-bias-baseline-config")]
    public string? ModelBiasBaselineConfig { get; set; }

    [CliOption("--network-config")]
    public string? NetworkConfig { get; set; }

    [CliOption("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}