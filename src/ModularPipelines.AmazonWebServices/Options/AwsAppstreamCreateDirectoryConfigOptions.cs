using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-directory-config")]
public record AwsAppstreamCreateDirectoryConfigOptions(
[property: CliOption("--directory-name")] string DirectoryName,
[property: CliOption("--organizational-unit-distinguished-names")] string[] OrganizationalUnitDistinguishedNames
) : AwsOptions
{
    [CliOption("--service-account-credentials")]
    public string? ServiceAccountCredentials { get; set; }

    [CliOption("--certificate-based-auth-properties")]
    public string? CertificateBasedAuthProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}