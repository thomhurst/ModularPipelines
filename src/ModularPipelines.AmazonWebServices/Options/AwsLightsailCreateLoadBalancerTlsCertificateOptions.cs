using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-load-balancer-tls-certificate")]
public record AwsLightsailCreateLoadBalancerTlsCertificateOptions(
[property: CommandSwitch("--load-balancer-name")] string LoadBalancerName,
[property: CommandSwitch("--certificate-name")] string CertificateName,
[property: CommandSwitch("--certificate-domain-name")] string CertificateDomainName
) : AwsOptions
{
    [CommandSwitch("--certificate-alternative-names")]
    public string[]? CertificateAlternativeNames { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}