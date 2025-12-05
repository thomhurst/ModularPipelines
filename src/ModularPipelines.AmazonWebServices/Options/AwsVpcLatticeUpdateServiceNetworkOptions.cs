using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "update-service-network")]
public record AwsVpcLatticeUpdateServiceNetworkOptions(
[property: CliOption("--auth-type")] string AuthType,
[property: CliOption("--service-network-identifier")] string ServiceNetworkIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}