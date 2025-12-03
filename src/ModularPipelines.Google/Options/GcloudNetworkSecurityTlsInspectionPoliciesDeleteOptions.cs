using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "tls-inspection-policies", "delete")]
public record GcloudNetworkSecurityTlsInspectionPoliciesDeleteOptions(
[property: CliArgument] string TlsInspectionPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}