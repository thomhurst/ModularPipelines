using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-directory-config")]
public record AwsAppstreamCreateDirectoryConfigOptions(
[property: CommandSwitch("--directory-name")] string DirectoryName,
[property: CommandSwitch("--organizational-unit-distinguished-names")] string[] OrganizationalUnitDistinguishedNames
) : AwsOptions
{
    [CommandSwitch("--service-account-credentials")]
    public string? ServiceAccountCredentials { get; set; }

    [CommandSwitch("--certificate-based-auth-properties")]
    public string? CertificateBasedAuthProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}