using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-load-balancer")]
public record AwsLightsailCreateLoadBalancerOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--instance-port")] int InstancePort
) : AwsOptions
{
    [CliOption("--health-check-path")]
    public string? HealthCheckPath { get; set; }

    [CliOption("--certificate-name")]
    public string? CertificateName { get; set; }

    [CliOption("--certificate-domain-name")]
    public string? CertificateDomainName { get; set; }

    [CliOption("--certificate-alternative-names")]
    public string[]? CertificateAlternativeNames { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--tls-policy-name")]
    public string? TlsPolicyName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}