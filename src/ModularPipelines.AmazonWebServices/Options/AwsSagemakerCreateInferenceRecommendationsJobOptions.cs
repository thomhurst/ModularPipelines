using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-inference-recommendations-job")]
public record AwsSagemakerCreateInferenceRecommendationsJobOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--job-type")] string JobType,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--input-config")] string InputConfig
) : AwsOptions
{
    [CliOption("--job-description")]
    public string? JobDescription { get; set; }

    [CliOption("--stopping-conditions")]
    public string? StoppingConditions { get; set; }

    [CliOption("--output-config")]
    public string? OutputConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}