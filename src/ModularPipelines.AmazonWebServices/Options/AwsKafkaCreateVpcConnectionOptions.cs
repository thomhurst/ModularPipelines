using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "create-vpc-connection")]
public record AwsKafkaCreateVpcConnectionOptions(
[property: CommandSwitch("--target-cluster-arn")] string TargetClusterArn,
[property: CommandSwitch("--authentication")] string Authentication,
[property: CommandSwitch("--vpc-id")] string VpcId,
[property: CommandSwitch("--client-subnets")] string[] ClientSubnets,
[property: CommandSwitch("--security-groups")] string[] SecurityGroups
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}