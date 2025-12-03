using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "get-scaling-configuration-recommendation")]
public record AwsSagemakerGetScalingConfigurationRecommendationOptions(
[property: CliOption("--inference-recommendations-job-name")] string InferenceRecommendationsJobName
) : AwsOptions
{
    [CliOption("--recommendation-id")]
    public string? RecommendationId { get; set; }

    [CliOption("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CliOption("--target-cpu-utilization-per-core")]
    public int? TargetCpuUtilizationPerCore { get; set; }

    [CliOption("--scaling-policy-objective")]
    public string? ScalingPolicyObjective { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}