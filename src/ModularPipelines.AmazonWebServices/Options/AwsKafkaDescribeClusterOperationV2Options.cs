using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "describe-cluster-operation-v2")]
public record AwsKafkaDescribeClusterOperationV2Options(
[property: CommandSwitch("--cluster-operation-arn")] string ClusterOperationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}