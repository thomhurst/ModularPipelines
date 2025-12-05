using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "get-compatible-kafka-versions")]
public record AwsKafkaGetCompatibleKafkaVersionsOptions : AwsOptions
{
    [CliOption("--cluster-arn")]
    public string? ClusterArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}