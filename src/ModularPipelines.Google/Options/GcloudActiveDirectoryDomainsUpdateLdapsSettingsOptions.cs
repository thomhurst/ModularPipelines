using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "update-ldaps-settings")]
public record GcloudActiveDirectoryDomainsUpdateLdapsSettingsOptions(
[property: CliArgument] string Domain,
[property: CliFlag("--clear-certificates")] bool ClearCertificates,
[property: CliOption("--certificate-pfx-file")] string CertificatePfxFile,
[property: CliOption("--certificate-password")] string CertificatePassword
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}