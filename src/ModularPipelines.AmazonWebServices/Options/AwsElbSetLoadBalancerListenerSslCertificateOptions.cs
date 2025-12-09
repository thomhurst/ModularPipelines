using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "set-load-balancer-listener-ssl-certificate")]
public record AwsElbSetLoadBalancerListenerSslCertificateOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--load-balancer-port")] int LoadBalancerPort,
[property: CliOption("--ssl-certificate-id")] string SslCertificateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}