using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "update-cluster-configuration")]
public record AwsKafkaUpdateClusterConfigurationOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--configuration-info")] string ConfigurationInfo,
[property: CliOption("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}