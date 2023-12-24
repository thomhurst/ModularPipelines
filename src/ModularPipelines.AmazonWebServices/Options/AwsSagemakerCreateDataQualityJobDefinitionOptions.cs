using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-data-quality-job-definition")]
public record AwsSagemakerCreateDataQualityJobDefinitionOptions(
[property: CommandSwitch("--job-definition-name")] string JobDefinitionName,
[property: CommandSwitch("--data-quality-app-specification")] string DataQualityAppSpecification,
[property: CommandSwitch("--data-quality-job-input")] string DataQualityJobInput,
[property: CommandSwitch("--data-quality-job-output-config")] string DataQualityJobOutputConfig,
[property: CommandSwitch("--job-resources")] string JobResources,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--data-quality-baseline-config")]
    public string? DataQualityBaselineConfig { get; set; }

    [CommandSwitch("--network-config")]
    public string? NetworkConfig { get; set; }

    [CommandSwitch("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}