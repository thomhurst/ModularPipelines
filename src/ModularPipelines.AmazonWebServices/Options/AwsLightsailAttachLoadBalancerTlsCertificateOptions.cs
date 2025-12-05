using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "attach-load-balancer-tls-certificate")]
public record AwsLightsailAttachLoadBalancerTlsCertificateOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--certificate-name")] string CertificateName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}