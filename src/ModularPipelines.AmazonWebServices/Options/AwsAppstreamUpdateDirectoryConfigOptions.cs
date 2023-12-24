using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "update-directory-config")]
public record AwsAppstreamUpdateDirectoryConfigOptions(
[property: CommandSwitch("--directory-name")] string DirectoryName
) : AwsOptions
{
    [CommandSwitch("--organizational-unit-distinguished-names")]
    public string[]? OrganizationalUnitDistinguishedNames { get; set; }

    [CommandSwitch("--service-account-credentials")]
    public string? ServiceAccountCredentials { get; set; }

    [CommandSwitch("--certificate-based-auth-properties")]
    public string? CertificateBasedAuthProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}