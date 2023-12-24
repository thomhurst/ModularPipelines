using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-queue-max-contacts")]
public record AwsConnectUpdateQueueMaxContactsOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--queue-id")] string QueueId
) : AwsOptions
{
    [CommandSwitch("--max-contacts")]
    public int? MaxContacts { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}