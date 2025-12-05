using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "upload-server-certificate")]
public record AwsIamUploadServerCertificateOptions(
[property: CliOption("--server-certificate-name")] string ServerCertificateName,
[property: CliOption("--certificate-body")] string CertificateBody,
[property: CliOption("--private-key")] string PrivateKey
) : AwsOptions
{
    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--certificate-chain")]
    public string? CertificateChain { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}