using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "tls-inspection-policies", "delete")]
public record GcloudNetworkSecurityTlsInspectionPoliciesDeleteOptions(
[property: PositionalArgument] string TlsInspectionPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}