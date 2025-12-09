using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-cluster-node")]
public record AwsSagemakerDescribeClusterNodeOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--node-id")] string NodeId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}