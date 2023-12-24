using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "update-broker-storage")]
public record AwsKafkaUpdateBrokerStorageOptions(
[property: CommandSwitch("--cluster-arn")] string ClusterArn,
[property: CommandSwitch("--current-version")] string CurrentVersion,
[property: CommandSwitch("--target-broker-ebs-volume-info")] string[] TargetBrokerEbsVolumeInfo
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}