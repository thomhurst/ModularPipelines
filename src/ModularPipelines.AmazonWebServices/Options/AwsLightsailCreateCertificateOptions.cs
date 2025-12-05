using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-certificate")]
public record AwsLightsailCreateCertificateOptions(
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--subject-alternative-names")]
    public string[]? SubjectAlternativeNames { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}