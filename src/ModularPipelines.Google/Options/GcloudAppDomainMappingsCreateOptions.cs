using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "domain-mappings", "create")]
public record GcloudAppDomainMappingsCreateOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions
{
    [CommandSwitch("--certificate-id")]
    public string? CertificateId { get; set; }

    [CommandSwitch("--certificate-management")]
    public string? CertificateManagement { get; set; }
}