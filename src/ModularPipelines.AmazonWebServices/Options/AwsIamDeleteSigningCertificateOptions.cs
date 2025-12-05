using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "delete-signing-certificate")]
public record AwsIamDeleteSigningCertificateOptions(
[property: CliOption("--certificate-id")] string CertificateId
) : AwsOptions
{
    [CliOption("--user-name")]
    public string? UserName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}