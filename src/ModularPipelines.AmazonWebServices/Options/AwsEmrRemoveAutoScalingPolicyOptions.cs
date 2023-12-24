using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "remove-auto-scaling-policy")]
public record AwsEmrRemoveAutoScalingPolicyOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--instance-group-id")] string InstanceGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}