using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm", "resend-validation-email")]
public record AwsAcmResendValidationEmailOptions(
[property: CliOption("--certificate-arn")] string CertificateArn,
[property: CliOption("--domain")] string Domain,
[property: CliOption("--validation-domain")] string ValidationDomain
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}