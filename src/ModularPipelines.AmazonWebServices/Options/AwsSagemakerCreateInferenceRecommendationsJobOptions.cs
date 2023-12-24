using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-inference-recommendations-job")]
public record AwsSagemakerCreateInferenceRecommendationsJobOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--job-type")] string JobType,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--input-config")] string InputConfig
) : AwsOptions
{
    [CommandSwitch("--job-description")]
    public string? JobDescription { get; set; }

    [CommandSwitch("--stopping-conditions")]
    public string? StoppingConditions { get; set; }

    [CommandSwitch("--output-config")]
    public string? OutputConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}