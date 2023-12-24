using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-model-bias-job-definition")]
public record AwsSagemakerCreateModelBiasJobDefinitionOptions(
[property: CommandSwitch("--job-definition-name")] string JobDefinitionName,
[property: CommandSwitch("--model-bias-app-specification")] string ModelBiasAppSpecification,
[property: CommandSwitch("--model-bias-job-input")] string ModelBiasJobInput,
[property: CommandSwitch("--model-bias-job-output-config")] string ModelBiasJobOutputConfig,
[property: CommandSwitch("--job-resources")] string JobResources,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--model-bias-baseline-config")]
    public string? ModelBiasBaselineConfig { get; set; }

    [CommandSwitch("--network-config")]
    public string? NetworkConfig { get; set; }

    [CommandSwitch("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}