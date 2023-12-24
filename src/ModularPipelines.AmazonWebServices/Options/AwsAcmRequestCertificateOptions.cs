using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm", "request-certificate")]
public record AwsAcmRequestCertificateOptions(
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--validation-method")]
    public string? ValidationMethod { get; set; }

    [CommandSwitch("--subject-alternative-names")]
    public string[]? SubjectAlternativeNames { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--domain-validation-options")]
    public string[]? DomainValidationOptions { get; set; }

    [CommandSwitch("--options")]
    public string? Options { get; set; }

    [CommandSwitch("--certificate-authority-arn")]
    public string? CertificateAuthorityArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--key-algorithm")]
    public string? KeyAlgorithm { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}