using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "update-broker-count")]
public record AwsKafkaUpdateBrokerCountOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--current-version")] string CurrentVersion,
[property: CliOption("--target-number-of-broker-nodes")] int TargetNumberOfBrokerNodes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}