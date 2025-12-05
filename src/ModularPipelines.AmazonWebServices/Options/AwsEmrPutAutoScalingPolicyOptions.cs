using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "put-auto-scaling-policy")]
public record AwsEmrPutAutoScalingPolicyOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--instance-group-id")] string InstanceGroupId,
[property: CliOption("--auto-scaling-policy")] string AutoScalingPolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}