using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "issuance-configs", "list")]
public record GcloudCertificateManagerIssuanceConfigsListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}