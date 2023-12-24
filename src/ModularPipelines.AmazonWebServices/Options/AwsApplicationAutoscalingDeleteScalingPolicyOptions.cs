using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-autoscaling", "delete-scaling-policy")]
public record AwsApplicationAutoscalingDeleteScalingPolicyOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--service-namespace")] string ServiceNamespace,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--scalable-dimension")] string ScalableDimension
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}