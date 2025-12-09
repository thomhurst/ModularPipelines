using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "unassign-private-ip-addresses")]
public record AwsEc2UnassignPrivateIpAddressesOptions(
[property: CliOption("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CliOption("--private-ip-addresses")]
    public string[]? PrivateIpAddresses { get; set; }

    [CliOption("--ipv4-prefixes")]
    public string[]? Ipv4Prefixes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}