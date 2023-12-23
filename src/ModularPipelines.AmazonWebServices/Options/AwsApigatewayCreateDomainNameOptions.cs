using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "create-domain-name")]
public record AwsApigatewayCreateDomainNameOptions(
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--certificate-name")]
    public string? CertificateName { get; set; }

    [CommandSwitch("--certificate-body")]
    public string? CertificateBody { get; set; }

    [CommandSwitch("--certificate-private-key")]
    public string? CertificatePrivateKey { get; set; }

    [CommandSwitch("--certificate-chain")]
    public string? CertificateChain { get; set; }

    [CommandSwitch("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CommandSwitch("--regional-certificate-name")]
    public string? RegionalCertificateName { get; set; }

    [CommandSwitch("--regional-certificate-arn")]
    public string? RegionalCertificateArn { get; set; }

    [CommandSwitch("--endpoint-configuration")]
    public string? EndpointConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CommandSwitch("--mutual-tls-authentication")]
    public string? MutualTlsAuthentication { get; set; }

    [CommandSwitch("--ownership-verification-certificate-arn")]
    public string? OwnershipVerificationCertificateArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}