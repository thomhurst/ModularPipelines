using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "domain-mappings", "update")]
public record GcloudAppDomainMappingsUpdateOptions(
[property: CliArgument] string Domain
) : GcloudOptions
{
    [CliOption("--certificate-management")]
    public string? CertificateManagement { get; set; }

    [CliOption("--certificate-id")]
    public string? CertificateId { get; set; }

    [CliFlag("--no-certificate-id")]
    public bool? NoCertificateId { get; set; }
}