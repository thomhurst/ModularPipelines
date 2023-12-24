using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "provision-ipam-pool-cidr")]
public record AwsEc2ProvisionIpamPoolCidrOptions(
[property: CommandSwitch("--ipam-pool-id")] string IpamPoolId
) : AwsOptions
{
    [CommandSwitch("--cidr")]
    public string? Cidr { get; set; }

    [CommandSwitch("--cidr-authorization-context")]
    public string? CidrAuthorizationContext { get; set; }

    [CommandSwitch("--netmask-length")]
    public int? NetmaskLength { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}