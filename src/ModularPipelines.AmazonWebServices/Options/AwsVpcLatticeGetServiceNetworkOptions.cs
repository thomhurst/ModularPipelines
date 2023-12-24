using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "get-service-network")]
public record AwsVpcLatticeGetServiceNetworkOptions(
[property: CommandSwitch("--service-network-identifier")] string ServiceNetworkIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}