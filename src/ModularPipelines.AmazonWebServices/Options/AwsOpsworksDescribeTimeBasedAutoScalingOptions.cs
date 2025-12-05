using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "describe-time-based-auto-scaling")]
public record AwsOpsworksDescribeTimeBasedAutoScalingOptions(
[property: CliOption("--instance-ids")] string[] InstanceIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}