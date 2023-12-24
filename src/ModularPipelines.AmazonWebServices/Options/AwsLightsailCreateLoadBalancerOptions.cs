using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-load-balancer")]
public record AwsLightsailCreateLoadBalancerOptions(
[property: CommandSwitch("--load-balancer-name")] string LoadBalancerName,
[property: CommandSwitch("--instance-port")] int InstancePort
) : AwsOptions
{
    [CommandSwitch("--health-check-path")]
    public string? HealthCheckPath { get; set; }

    [CommandSwitch("--certificate-name")]
    public string? CertificateName { get; set; }

    [CommandSwitch("--certificate-domain-name")]
    public string? CertificateDomainName { get; set; }

    [CommandSwitch("--certificate-alternative-names")]
    public string[]? CertificateAlternativeNames { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CommandSwitch("--tls-policy-name")]
    public string? TlsPolicyName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}