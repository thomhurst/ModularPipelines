using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "domain-mappings", "create")]
public record GcloudAppDomainMappingsCreateOptions(
[property: CliArgument] string Domain
) : GcloudOptions
{
    [CliOption("--certificate-id")]
    public string? CertificateId { get; set; }

    [CliOption("--certificate-management")]
    public string? CertificateManagement { get; set; }
}