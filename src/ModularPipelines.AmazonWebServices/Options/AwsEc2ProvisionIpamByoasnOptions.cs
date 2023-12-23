using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "provision-ipam-byoasn")]
public record AwsEc2ProvisionIpamByoasnOptions(
[property: CommandSwitch("--ipam-id")] string IpamId,
[property: CommandSwitch("--asn")] string Asn,
[property: CommandSwitch("--asn-authorization-context")] string AsnAuthorizationContext
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}