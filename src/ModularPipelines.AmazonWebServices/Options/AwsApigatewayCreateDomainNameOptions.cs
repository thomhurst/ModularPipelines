using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "create-domain-name")]
public record AwsApigatewayCreateDomainNameOptions(
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--certificate-name")]
    public string? CertificateName { get; set; }

    [CliOption("--certificate-body")]
    public string? CertificateBody { get; set; }

    [CliOption("--certificate-private-key")]
    public string? CertificatePrivateKey { get; set; }

    [CliOption("--certificate-chain")]
    public string? CertificateChain { get; set; }

    [CliOption("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CliOption("--regional-certificate-name")]
    public string? RegionalCertificateName { get; set; }

    [CliOption("--regional-certificate-arn")]
    public string? RegionalCertificateArn { get; set; }

    [CliOption("--endpoint-configuration")]
    public string? EndpointConfiguration { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CliOption("--mutual-tls-authentication")]
    public string? MutualTlsAuthentication { get; set; }

    [CliOption("--ownership-verification-certificate-arn")]
    public string? OwnershipVerificationCertificateArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}