using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "get-ipam-discovered-public-addresses")]
public record AwsEc2GetIpamDiscoveredPublicAddressesOptions(
[property: CommandSwitch("--ipam-resource-discovery-id")] string IpamResourceDiscoveryId,
[property: CommandSwitch("--address-region")] string AddressRegion
) : AwsOptions
{
    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}