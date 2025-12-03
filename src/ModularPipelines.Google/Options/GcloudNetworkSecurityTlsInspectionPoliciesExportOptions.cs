using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "tls-inspection-policies", "export")]
public record GcloudNetworkSecurityTlsInspectionPoliciesExportOptions(
[property: CliArgument] string TlsInspectionPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}