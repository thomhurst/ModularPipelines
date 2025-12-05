using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-load-balancer-tls-certificate")]
public record AwsLightsailCreateLoadBalancerTlsCertificateOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliOption("--certificate-domain-name")] string CertificateDomainName
) : AwsOptions
{
    [CliOption("--certificate-alternative-names")]
    public string[]? CertificateAlternativeNames { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}