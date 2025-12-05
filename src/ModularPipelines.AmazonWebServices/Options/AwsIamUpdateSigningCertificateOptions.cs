using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "update-signing-certificate")]
public record AwsIamUpdateSigningCertificateOptions(
[property: CliOption("--certificate-id")] string CertificateId,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--user-name")]
    public string? UserName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}