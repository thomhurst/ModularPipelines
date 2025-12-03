using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm", "request-certificate")]
public record AwsAcmRequestCertificateOptions(
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--validation-method")]
    public string? ValidationMethod { get; set; }

    [CliOption("--subject-alternative-names")]
    public string[]? SubjectAlternativeNames { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--domain-validation-options")]
    public string[]? DomainValidationOptions { get; set; }

    [CliOption("--options")]
    public string? Options { get; set; }

    [CliOption("--certificate-authority-arn")]
    public string? CertificateAuthorityArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--key-algorithm")]
    public string? KeyAlgorithm { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}