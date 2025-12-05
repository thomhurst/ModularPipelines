using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "update-cluster-kafka-version")]
public record AwsKafkaUpdateClusterKafkaVersionOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--current-version")] string CurrentVersion,
[property: CliOption("--target-kafka-version")] string TargetKafkaVersion
) : AwsOptions
{
    [CliOption("--configuration-info")]
    public string? ConfigurationInfo { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}