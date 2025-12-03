using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "update-broker-type")]
public record AwsKafkaUpdateBrokerTypeOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--current-version")] string CurrentVersion,
[property: CliOption("--target-instance-type")] string TargetInstanceType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}