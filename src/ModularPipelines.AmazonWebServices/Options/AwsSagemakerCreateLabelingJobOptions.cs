using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-labeling-job")]
public record AwsSagemakerCreateLabelingJobOptions(
[property: CommandSwitch("--labeling-job-name")] string LabelingJobName,
[property: CommandSwitch("--label-attribute-name")] string LabelAttributeName,
[property: CommandSwitch("--input-config")] string InputConfig,
[property: CommandSwitch("--output-config")] string OutputConfig,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--human-task-config")] string HumanTaskConfig
) : AwsOptions
{
    [CommandSwitch("--label-category-config-s3-uri")]
    public string? LabelCategoryConfigS3Uri { get; set; }

    [CommandSwitch("--stopping-conditions")]
    public string? StoppingConditions { get; set; }

    [CommandSwitch("--labeling-job-algorithms-config")]
    public string? LabelingJobAlgorithmsConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}