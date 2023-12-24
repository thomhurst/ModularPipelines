using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "get-compatible-kafka-versions")]
public record AwsKafkaGetCompatibleKafkaVersionsOptions : AwsOptions
{
    [CommandSwitch("--cluster-arn")]
    public string? ClusterArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}