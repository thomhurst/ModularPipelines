using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "tls-inspection-policies", "export")]
public record GcloudNetworkSecurityTlsInspectionPoliciesExportOptions(
[property: PositionalArgument] string TlsInspectionPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}