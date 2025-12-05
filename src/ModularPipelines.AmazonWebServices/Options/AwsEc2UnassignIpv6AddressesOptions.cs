using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "unassign-ipv6-addresses")]
public record AwsEc2UnassignIpv6AddressesOptions(
[property: CliOption("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CliOption("--ipv6-addresses")]
    public string[]? Ipv6Addresses { get; set; }

    [CliOption("--ipv6-prefixes")]
    public string[]? Ipv6Prefixes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}