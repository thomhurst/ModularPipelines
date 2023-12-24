using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "create-vpc-peering-connection")]
public record AwsGameliftCreateVpcPeeringConnectionOptions(
[property: CommandSwitch("--fleet-id")] string FleetId,
[property: CommandSwitch("--peer-vpc-aws-account-id")] string PeerVpcAwsAccountId,
[property: CommandSwitch("--peer-vpc-id")] string PeerVpcId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}