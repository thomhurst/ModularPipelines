using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-network-interface")]
public record AwsEc2CreateNetworkInterfaceOptions(
[property: CommandSwitch("--subnet-id")] string SubnetId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--groups")]
    public string[]? Groups { get; set; }

    [CommandSwitch("--ipv6-address-count")]
    public int? Ipv6AddressCount { get; set; }

    [CommandSwitch("--ipv6-addresses")]
    public string[]? Ipv6Addresses { get; set; }

    [CommandSwitch("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CommandSwitch("--private-ip-addresses")]
    public string[]? PrivateIpAddresses { get; set; }

    [CommandSwitch("--secondary-private-ip-address-count")]
    public int? SecondaryPrivateIpAddressCount { get; set; }

    [CommandSwitch("--ipv4-prefixes")]
    public string[]? Ipv4Prefixes { get; set; }

    [CommandSwitch("--ipv4-prefix-count")]
    public int? Ipv4PrefixCount { get; set; }

    [CommandSwitch("--ipv6-prefixes")]
    public string[]? Ipv6Prefixes { get; set; }

    [CommandSwitch("--ipv6-prefix-count")]
    public int? Ipv6PrefixCount { get; set; }

    [CommandSwitch("--interface-type")]
    public string? InterfaceType { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--connection-tracking-specification")]
    public string? ConnectionTrackingSpecification { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}