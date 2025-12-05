using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "delete-service-network-service-association")]
public record AwsVpcLatticeDeleteServiceNetworkServiceAssociationOptions(
[property: CliOption("--service-network-service-association-identifier")] string ServiceNetworkServiceAssociationIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}