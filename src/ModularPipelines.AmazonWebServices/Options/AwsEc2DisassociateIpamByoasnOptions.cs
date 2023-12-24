using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "disassociate-ipam-byoasn")]
public record AwsEc2DisassociateIpamByoasnOptions(
[property: CommandSwitch("--asn")] string Asn,
[property: CommandSwitch("--cidr")] string Cidr
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}