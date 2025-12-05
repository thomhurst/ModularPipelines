using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "put-cluster-policy")]
public record AwsKafkaPutClusterPolicyOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--policy")] string Policy
) : AwsOptions
{
    [CliOption("--current-version")]
    public string? CurrentVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}