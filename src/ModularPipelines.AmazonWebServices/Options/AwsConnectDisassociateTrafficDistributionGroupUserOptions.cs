using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "disassociate-traffic-distribution-group-user")]
public record AwsConnectDisassociateTrafficDistributionGroupUserOptions(
[property: CommandSwitch("--traffic-distribution-group-id")] string TrafficDistributionGroupId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}