using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "assign-private-ip-addresses")]
public record AwsEc2AssignPrivateIpAddressesOptions(
[property: CommandSwitch("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CommandSwitch("--private-ip-addresses")]
    public string[]? PrivateIpAddresses { get; set; }

    [CommandSwitch("--secondary-private-ip-address-count")]
    public int? SecondaryPrivateIpAddressCount { get; set; }

    [CommandSwitch("--ipv4-prefixes")]
    public string[]? Ipv4Prefixes { get; set; }

    [CommandSwitch("--ipv4-prefix-count")]
    public int? Ipv4PrefixCount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}