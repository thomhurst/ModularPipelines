using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "update-broker-type")]
public record AwsKafkaUpdateBrokerTypeOptions(
[property: CommandSwitch("--cluster-arn")] string ClusterArn,
[property: CommandSwitch("--current-version")] string CurrentVersion,
[property: CommandSwitch("--target-instance-type")] string TargetInstanceType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}