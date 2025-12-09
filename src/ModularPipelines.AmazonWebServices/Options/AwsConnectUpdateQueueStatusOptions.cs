using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-queue-status")]
public record AwsConnectUpdateQueueStatusOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--queue-id")] string QueueId,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}