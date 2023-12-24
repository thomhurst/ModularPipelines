using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-queue-hours-of-operation")]
public record AwsConnectUpdateQueueHoursOfOperationOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--queue-id")] string QueueId,
[property: CommandSwitch("--hours-of-operation-id")] string HoursOfOperationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}