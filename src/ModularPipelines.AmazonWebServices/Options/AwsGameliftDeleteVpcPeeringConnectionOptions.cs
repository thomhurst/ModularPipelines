using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "delete-vpc-peering-connection")]
public record AwsGameliftDeleteVpcPeeringConnectionOptions(
[property: CommandSwitch("--fleet-id")] string FleetId,
[property: CommandSwitch("--vpc-peering-connection-id")] string VpcPeeringConnectionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}