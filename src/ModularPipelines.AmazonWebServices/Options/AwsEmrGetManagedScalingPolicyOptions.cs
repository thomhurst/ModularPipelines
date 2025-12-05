using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "get-managed-scaling-policy")]
public record AwsEmrGetManagedScalingPolicyOptions(
[property: CliOption("--cluster-id")] string ClusterId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}