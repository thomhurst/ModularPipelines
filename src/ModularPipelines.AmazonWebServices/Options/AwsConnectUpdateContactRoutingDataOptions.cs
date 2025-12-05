using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-contact-routing-data")]
public record AwsConnectUpdateContactRoutingDataOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-id")] string ContactId
) : AwsOptions
{
    [CliOption("--queue-time-adjustment-seconds")]
    public int? QueueTimeAdjustmentSeconds { get; set; }

    [CliOption("--queue-priority")]
    public long? QueuePriority { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}