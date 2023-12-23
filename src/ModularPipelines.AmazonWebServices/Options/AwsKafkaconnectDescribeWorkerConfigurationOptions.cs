using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafkaconnect", "describe-worker-configuration")]
public record AwsKafkaconnectDescribeWorkerConfigurationOptions(
[property: CommandSwitch("--worker-configuration-arn")] string WorkerConfigurationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}