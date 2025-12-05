using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafkaconnect", "describe-worker-configuration")]
public record AwsKafkaconnectDescribeWorkerConfigurationOptions(
[property: CliOption("--worker-configuration-arn")] string WorkerConfigurationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}