using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "update-connectivity")]
public record AwsKafkaUpdateConnectivityOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--connectivity-info")] string ConnectivityInfo,
[property: CliOption("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}