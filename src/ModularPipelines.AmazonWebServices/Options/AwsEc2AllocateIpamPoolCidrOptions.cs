using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "allocate-ipam-pool-cidr")]
public record AwsEc2AllocateIpamPoolCidrOptions(
[property: CommandSwitch("--ipam-pool-id")] string IpamPoolId
) : AwsOptions
{
    [CommandSwitch("--cidr")]
    public string? Cidr { get; set; }

    [CommandSwitch("--netmask-length")]
    public int? NetmaskLength { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--allowed-cidrs")]
    public string[]? AllowedCidrs { get; set; }

    [CommandSwitch("--disallowed-cidrs")]
    public string[]? DisallowedCidrs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}