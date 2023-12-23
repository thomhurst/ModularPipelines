using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "unassign-ipv6-addresses")]
public record AwsEc2UnassignIpv6AddressesOptions(
[property: CommandSwitch("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CommandSwitch("--ipv6-addresses")]
    public string[]? Ipv6Addresses { get; set; }

    [CommandSwitch("--ipv6-prefixes")]
    public string[]? Ipv6Prefixes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}