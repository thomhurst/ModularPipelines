using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "update-cluster-kafka-version")]
public record AwsKafkaUpdateClusterKafkaVersionOptions(
[property: CommandSwitch("--cluster-arn")] string ClusterArn,
[property: CommandSwitch("--current-version")] string CurrentVersion,
[property: CommandSwitch("--target-kafka-version")] string TargetKafkaVersion
) : AwsOptions
{
    [CommandSwitch("--configuration-info")]
    public string? ConfigurationInfo { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}