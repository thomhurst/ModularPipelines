using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "allocate-address")]
public record AwsEc2AllocateAddressOptions : AwsOptions
{
    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--public-ipv4-pool")]
    public string? PublicIpv4Pool { get; set; }

    [CommandSwitch("--network-border-group")]
    public string? NetworkBorderGroup { get; set; }

    [CommandSwitch("--customer-owned-ipv4-pool")]
    public string? CustomerOwnedIpv4Pool { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}