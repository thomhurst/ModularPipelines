using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "describe-certificate")]
public record AwsDsDescribeCertificateOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--certificate-id")] string CertificateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}