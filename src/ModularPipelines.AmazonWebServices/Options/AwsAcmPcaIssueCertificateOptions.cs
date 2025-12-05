using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm-pca", "issue-certificate")]
public record AwsAcmPcaIssueCertificateOptions(
[property: CliOption("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CliOption("--csr")] string Csr,
[property: CliOption("--signing-algorithm")] string SigningAlgorithm,
[property: CliOption("--validity")] string Validity
) : AwsOptions
{
    [CliOption("--api-passthrough")]
    public string? ApiPassthrough { get; set; }

    [CliOption("--template-arn")]
    public string? TemplateArn { get; set; }

    [CliOption("--validity-not-before")]
    public string? ValidityNotBefore { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}