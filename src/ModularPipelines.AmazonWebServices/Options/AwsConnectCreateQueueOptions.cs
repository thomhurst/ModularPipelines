using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-queue")]
public record AwsConnectCreateQueueOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--hours-of-operation-id")] string HoursOfOperationId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--outbound-caller-config")]
    public string? OutboundCallerConfig { get; set; }

    [CommandSwitch("--max-contacts")]
    public int? MaxContacts { get; set; }

    [CommandSwitch("--quick-connect-ids")]
    public string[]? QuickConnectIds { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}