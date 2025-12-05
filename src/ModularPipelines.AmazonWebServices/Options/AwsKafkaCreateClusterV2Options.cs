using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "create-cluster-v2")]
public record AwsKafkaCreateClusterV2Options(
[property: CliOption("--cluster-name")] string ClusterName
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--provisioned")]
    public string? Provisioned { get; set; }

    [CliOption("--serverless")]
    public string? Serverless { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}