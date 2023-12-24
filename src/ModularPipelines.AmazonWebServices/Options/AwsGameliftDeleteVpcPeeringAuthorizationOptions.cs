using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "delete-vpc-peering-authorization")]
public record AwsGameliftDeleteVpcPeeringAuthorizationOptions(
[property: CommandSwitch("--game-lift-aws-account-id")] string GameLiftAwsAccountId,
[property: CommandSwitch("--peer-vpc-id")] string PeerVpcId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}