using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "get-service-network-vpc-association")]
public record AwsVpcLatticeGetServiceNetworkVpcAssociationOptions(
[property: CommandSwitch("--service-network-vpc-association-identifier")] string ServiceNetworkVpcAssociationIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}