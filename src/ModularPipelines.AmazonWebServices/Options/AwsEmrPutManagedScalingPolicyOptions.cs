using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "put-managed-scaling-policy")]
public record AwsEmrPutManagedScalingPolicyOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--managed-scaling-policy")] string ManagedScalingPolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}