using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "reject-client-vpc-connection")]
public record AwsKafkaRejectClientVpcConnectionOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--vpc-connection-arn")] string VpcConnectionArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}