using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "domain-mappings", "update")]
public record GcloudAppDomainMappingsUpdateOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions
{
    [CommandSwitch("--certificate-management")]
    public string? CertificateManagement { get; set; }

    [CommandSwitch("--certificate-id")]
    public string? CertificateId { get; set; }

    [BooleanCommandSwitch("--no-certificate-id")]
    public bool? NoCertificateId { get; set; }
}