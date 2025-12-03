using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "get-ipam-discovered-public-addresses")]
public record AwsEc2GetIpamDiscoveredPublicAddressesOptions(
[property: CliOption("--ipam-resource-discovery-id")] string IpamResourceDiscoveryId,
[property: CliOption("--address-region")] string AddressRegion
) : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}