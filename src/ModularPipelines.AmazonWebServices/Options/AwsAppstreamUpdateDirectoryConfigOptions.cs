using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "update-directory-config")]
public record AwsAppstreamUpdateDirectoryConfigOptions(
[property: CliOption("--directory-name")] string DirectoryName
) : AwsOptions
{
    [CliOption("--organizational-unit-distinguished-names")]
    public string[]? OrganizationalUnitDistinguishedNames { get; set; }

    [CliOption("--service-account-credentials")]
    public string? ServiceAccountCredentials { get; set; }

    [CliOption("--certificate-based-auth-properties")]
    public string? CertificateBasedAuthProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}