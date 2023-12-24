using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "delete-service-network-service-association")]
public record AwsVpcLatticeDeleteServiceNetworkServiceAssociationOptions(
[property: CommandSwitch("--service-network-service-association-identifier")] string ServiceNetworkServiceAssociationIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}