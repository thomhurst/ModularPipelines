using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-routing-profile-queues")]
public record AwsConnectUpdateRoutingProfileQueuesOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--routing-profile-id")] string RoutingProfileId,
[property: CommandSwitch("--queue-configs")] string[] QueueConfigs
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}