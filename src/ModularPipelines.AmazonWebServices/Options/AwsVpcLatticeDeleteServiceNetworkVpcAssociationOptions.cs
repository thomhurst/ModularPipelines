using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "delete-service-network-vpc-association")]
public record AwsVpcLatticeDeleteServiceNetworkVpcAssociationOptions(
[property: CliOption("--service-network-vpc-association-identifier")] string ServiceNetworkVpcAssociationIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}