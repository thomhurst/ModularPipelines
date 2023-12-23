using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm-pca", "issue-certificate")]
public record AwsAcmPcaIssueCertificateOptions(
[property: CommandSwitch("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CommandSwitch("--csr")] string Csr,
[property: CommandSwitch("--signing-algorithm")] string SigningAlgorithm,
[property: CommandSwitch("--validity")] string Validity
) : AwsOptions
{
    [CommandSwitch("--api-passthrough")]
    public string? ApiPassthrough { get; set; }

    [CommandSwitch("--template-arn")]
    public string? TemplateArn { get; set; }

    [CommandSwitch("--validity-not-before")]
    public string? ValidityNotBefore { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}