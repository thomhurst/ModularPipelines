using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-autoscaling", "deregister-scalable-target")]
public record AwsApplicationAutoscalingDeregisterScalableTargetOptions(
[property: CommandSwitch("--service-namespace")] string ServiceNamespace,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--scalable-dimension")] string ScalableDimension
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}