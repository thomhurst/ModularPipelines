using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "delete-load-balancer-tls-certificate")]
public record AwsLightsailDeleteLoadBalancerTlsCertificateOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--certificate-name")] string CertificateName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}