using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "disassociate-routing-profile-queues")]
public record AwsConnectDisassociateRoutingProfileQueuesOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--routing-profile-id")] string RoutingProfileId,
[property: CommandSwitch("--queue-references")] string[] QueueReferences
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}