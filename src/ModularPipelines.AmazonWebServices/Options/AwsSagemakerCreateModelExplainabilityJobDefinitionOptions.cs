using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-model-explainability-job-definition")]
public record AwsSagemakerCreateModelExplainabilityJobDefinitionOptions(
[property: CommandSwitch("--job-definition-name")] string JobDefinitionName,
[property: CommandSwitch("--model-explainability-app-specification")] string ModelExplainabilityAppSpecification,
[property: CommandSwitch("--model-explainability-job-input")] string ModelExplainabilityJobInput,
[property: CommandSwitch("--model-explainability-job-output-config")] string ModelExplainabilityJobOutputConfig,
[property: CommandSwitch("--job-resources")] string JobResources,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--model-explainability-baseline-config")]
    public string? ModelExplainabilityBaselineConfig { get; set; }

    [CommandSwitch("--network-config")]
    public string? NetworkConfig { get; set; }

    [CommandSwitch("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}