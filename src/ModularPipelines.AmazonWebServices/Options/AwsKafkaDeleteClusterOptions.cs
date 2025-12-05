using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "delete-cluster")]
public record AwsKafkaDeleteClusterOptions(
[property: CliOption("--cluster-arn")] string ClusterArn
) : AwsOptions
{
    [CliOption("--current-version")]
    public string? CurrentVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}