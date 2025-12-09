using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "describe-nodegroup")]
public record AwsEksDescribeNodegroupOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--nodegroup-name")] string NodegroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}