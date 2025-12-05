using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-labeling-job")]
public record AwsSagemakerCreateLabelingJobOptions(
[property: CliOption("--labeling-job-name")] string LabelingJobName,
[property: CliOption("--label-attribute-name")] string LabelAttributeName,
[property: CliOption("--input-config")] string InputConfig,
[property: CliOption("--output-config")] string OutputConfig,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--human-task-config")] string HumanTaskConfig
) : AwsOptions
{
    [CliOption("--label-category-config-s3-uri")]
    public string? LabelCategoryConfigS3Uri { get; set; }

    [CliOption("--stopping-conditions")]
    public string? StoppingConditions { get; set; }

    [CliOption("--labeling-job-algorithms-config")]
    public string? LabelingJobAlgorithmsConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}