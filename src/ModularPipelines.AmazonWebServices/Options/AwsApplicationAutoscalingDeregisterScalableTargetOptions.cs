using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-autoscaling", "deregister-scalable-target")]
public record AwsApplicationAutoscalingDeregisterScalableTargetOptions(
[property: CliOption("--service-namespace")] string ServiceNamespace,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--scalable-dimension")] string ScalableDimension
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}