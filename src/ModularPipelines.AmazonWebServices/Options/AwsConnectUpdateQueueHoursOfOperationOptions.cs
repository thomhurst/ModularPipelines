using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-queue-hours-of-operation")]
public record AwsConnectUpdateQueueHoursOfOperationOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--queue-id")] string QueueId,
[property: CliOption("--hours-of-operation-id")] string HoursOfOperationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}