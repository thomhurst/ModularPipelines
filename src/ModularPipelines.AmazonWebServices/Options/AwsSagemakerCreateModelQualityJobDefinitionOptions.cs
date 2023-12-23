using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-model-quality-job-definition")]
public record AwsSagemakerCreateModelQualityJobDefinitionOptions(
[property: CommandSwitch("--job-definition-name")] string JobDefinitionName,
[property: CommandSwitch("--model-quality-app-specification")] string ModelQualityAppSpecification,
[property: CommandSwitch("--model-quality-job-input")] string ModelQualityJobInput,
[property: CommandSwitch("--model-quality-job-output-config")] string ModelQualityJobOutputConfig,
[property: CommandSwitch("--job-resources")] string JobResources,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--model-quality-baseline-config")]
    public string? ModelQualityBaselineConfig { get; set; }

    [CommandSwitch("--network-config")]
    public string? NetworkConfig { get; set; }

    [CommandSwitch("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}