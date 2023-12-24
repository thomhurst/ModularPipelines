using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "unassign-private-ip-addresses")]
public record AwsEc2UnassignPrivateIpAddressesOptions(
[property: CommandSwitch("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CommandSwitch("--private-ip-addresses")]
    public string[]? PrivateIpAddresses { get; set; }

    [CommandSwitch("--ipv4-prefixes")]
    public string[]? Ipv4Prefixes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}