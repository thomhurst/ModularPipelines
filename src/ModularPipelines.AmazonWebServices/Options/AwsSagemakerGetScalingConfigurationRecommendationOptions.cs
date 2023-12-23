using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "get-scaling-configuration-recommendation")]
public record AwsSagemakerGetScalingConfigurationRecommendationOptions(
[property: CommandSwitch("--inference-recommendations-job-name")] string InferenceRecommendationsJobName
) : AwsOptions
{
    [CommandSwitch("--recommendation-id")]
    public string? RecommendationId { get; set; }

    [CommandSwitch("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CommandSwitch("--target-cpu-utilization-per-core")]
    public int? TargetCpuUtilizationPerCore { get; set; }

    [CommandSwitch("--scaling-policy-objective")]
    public string? ScalingPolicyObjective { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}