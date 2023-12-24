using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "put-managed-scaling-policy")]
public record AwsEmrPutManagedScalingPolicyOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--managed-scaling-policy")] string ManagedScalingPolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}