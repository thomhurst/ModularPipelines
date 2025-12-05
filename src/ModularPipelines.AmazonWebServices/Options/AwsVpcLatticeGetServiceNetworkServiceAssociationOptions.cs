using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "get-service-network-service-association")]
public record AwsVpcLatticeGetServiceNetworkServiceAssociationOptions(
[property: CliOption("--service-network-service-association-identifier")] string ServiceNetworkServiceAssociationIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}