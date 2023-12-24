using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "update-service-network")]
public record AwsVpcLatticeUpdateServiceNetworkOptions(
[property: CommandSwitch("--auth-type")] string AuthType,
[property: CommandSwitch("--service-network-identifier")] string ServiceNetworkIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}