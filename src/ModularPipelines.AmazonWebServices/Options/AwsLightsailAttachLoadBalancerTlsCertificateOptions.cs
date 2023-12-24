using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "attach-load-balancer-tls-certificate")]
public record AwsLightsailAttachLoadBalancerTlsCertificateOptions(
[property: CommandSwitch("--load-balancer-name")] string LoadBalancerName,
[property: CommandSwitch("--certificate-name")] string CertificateName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}