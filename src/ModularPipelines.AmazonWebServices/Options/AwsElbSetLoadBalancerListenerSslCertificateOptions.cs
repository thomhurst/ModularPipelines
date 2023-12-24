using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elb", "set-load-balancer-listener-ssl-certificate")]
public record AwsElbSetLoadBalancerListenerSslCertificateOptions(
[property: CommandSwitch("--load-balancer-name")] string LoadBalancerName,
[property: CommandSwitch("--load-balancer-port")] int LoadBalancerPort,
[property: CommandSwitch("--ssl-certificate-id")] string SslCertificateId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}