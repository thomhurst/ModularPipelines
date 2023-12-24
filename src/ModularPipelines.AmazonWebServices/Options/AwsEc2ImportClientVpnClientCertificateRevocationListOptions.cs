using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "import-client-vpn-client-certificate-revocation-list")]
public record AwsEc2ImportClientVpnClientCertificateRevocationListOptions(
[property: CommandSwitch("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CommandSwitch("--certificate-revocation-list")] string CertificateRevocationList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}