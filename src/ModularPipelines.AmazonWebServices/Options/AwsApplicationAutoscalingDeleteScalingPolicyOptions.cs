using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-autoscaling", "delete-scaling-policy")]
public record AwsApplicationAutoscalingDeleteScalingPolicyOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--service-namespace")] string ServiceNamespace,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--scalable-dimension")] string ScalableDimension
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}