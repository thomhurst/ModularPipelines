using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "create-vpc-connection")]
public record AwsKafkaCreateVpcConnectionOptions(
[property: CliOption("--target-cluster-arn")] string TargetClusterArn,
[property: CliOption("--authentication")] string Authentication,
[property: CliOption("--vpc-id")] string VpcId,
[property: CliOption("--client-subnets")] string[] ClientSubnets,
[property: CliOption("--security-groups")] string[] SecurityGroups
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}