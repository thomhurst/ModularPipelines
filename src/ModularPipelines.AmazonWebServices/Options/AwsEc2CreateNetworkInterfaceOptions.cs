using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-network-interface")]
public record AwsEc2CreateNetworkInterfaceOptions(
[property: CliOption("--subnet-id")] string SubnetId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--groups")]
    public string[]? Groups { get; set; }

    [CliOption("--ipv6-address-count")]
    public int? Ipv6AddressCount { get; set; }

    [CliOption("--ipv6-addresses")]
    public string[]? Ipv6Addresses { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CliOption("--private-ip-addresses")]
    public string[]? PrivateIpAddresses { get; set; }

    [CliOption("--secondary-private-ip-address-count")]
    public int? SecondaryPrivateIpAddressCount { get; set; }

    [CliOption("--ipv4-prefixes")]
    public string[]? Ipv4Prefixes { get; set; }

    [CliOption("--ipv4-prefix-count")]
    public int? Ipv4PrefixCount { get; set; }

    [CliOption("--ipv6-prefixes")]
    public string[]? Ipv6Prefixes { get; set; }

    [CliOption("--ipv6-prefix-count")]
    public int? Ipv6PrefixCount { get; set; }

    [CliOption("--interface-type")]
    public string? InterfaceType { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--connection-tracking-specification")]
    public string? ConnectionTrackingSpecification { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}