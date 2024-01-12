using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "update-ldaps-settings")]
public record GcloudActiveDirectoryDomainsUpdateLdapsSettingsOptions(
[property: PositionalArgument] string Domain,
[property: BooleanCommandSwitch("--clear-certificates")] bool ClearCertificates,
[property: CommandSwitch("--certificate-pfx-file")] string CertificatePfxFile,
[property: CommandSwitch("--certificate-password")] string CertificatePassword
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}