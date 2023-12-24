using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-routing-profile-default-outbound-queue")]
public record AwsConnectUpdateRoutingProfileDefaultOutboundQueueOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--routing-profile-id")] string RoutingProfileId,
[property: CommandSwitch("--default-outbound-queue-id")] string DefaultOutboundQueueId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}