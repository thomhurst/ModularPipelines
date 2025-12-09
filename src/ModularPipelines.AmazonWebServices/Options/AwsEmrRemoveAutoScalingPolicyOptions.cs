using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "remove-auto-scaling-policy")]
public record AwsEmrRemoveAutoScalingPolicyOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--instance-group-id")] string InstanceGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}