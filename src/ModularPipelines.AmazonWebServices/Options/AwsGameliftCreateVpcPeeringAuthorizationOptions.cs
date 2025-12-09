using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "create-vpc-peering-authorization")]
public record AwsGameliftCreateVpcPeeringAuthorizationOptions(
[property: CliOption("--game-lift-aws-account-id")] string GameLiftAwsAccountId,
[property: CliOption("--peer-vpc-id")] string PeerVpcId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}