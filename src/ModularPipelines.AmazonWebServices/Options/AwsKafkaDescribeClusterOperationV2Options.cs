using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "describe-cluster-operation-v2")]
public record AwsKafkaDescribeClusterOperationV2Options(
[property: CliOption("--cluster-operation-arn")] string ClusterOperationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}