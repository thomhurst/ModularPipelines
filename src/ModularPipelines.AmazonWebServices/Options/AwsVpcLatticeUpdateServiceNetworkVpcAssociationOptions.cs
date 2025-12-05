using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "update-service-network-vpc-association")]
public record AwsVpcLatticeUpdateServiceNetworkVpcAssociationOptions(
[property: CliOption("--security-group-ids")] string[] SecurityGroupIds,
[property: CliOption("--service-network-vpc-association-identifier")] string ServiceNetworkVpcAssociationIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}