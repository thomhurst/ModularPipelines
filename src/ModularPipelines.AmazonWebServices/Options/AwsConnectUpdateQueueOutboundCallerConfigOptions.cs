using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-queue-outbound-caller-config")]
public record AwsConnectUpdateQueueOutboundCallerConfigOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--queue-id")] string QueueId,
[property: CliOption("--outbound-caller-config")] string OutboundCallerConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}