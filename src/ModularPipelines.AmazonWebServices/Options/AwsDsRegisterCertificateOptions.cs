using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "register-certificate")]
public record AwsDsRegisterCertificateOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--certificate-data")] string CertificateData
) : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--client-cert-auth-settings")]
    public string? ClientCertAuthSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}