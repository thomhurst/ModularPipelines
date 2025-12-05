using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "assign-private-ip-addresses")]
public record AwsEc2AssignPrivateIpAddressesOptions(
[property: CliOption("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CliOption("--private-ip-addresses")]
    public string[]? PrivateIpAddresses { get; set; }

    [CliOption("--secondary-private-ip-address-count")]
    public int? SecondaryPrivateIpAddressCount { get; set; }

    [CliOption("--ipv4-prefixes")]
    public string[]? Ipv4Prefixes { get; set; }

    [CliOption("--ipv4-prefix-count")]
    public int? Ipv4PrefixCount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}