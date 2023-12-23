using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "deprovision-ipam-byoasn")]
public record AwsEc2DeprovisionIpamByoasnOptions(
[property: CommandSwitch("--ipam-id")] string IpamId,
[property: CommandSwitch("--asn")] string Asn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}