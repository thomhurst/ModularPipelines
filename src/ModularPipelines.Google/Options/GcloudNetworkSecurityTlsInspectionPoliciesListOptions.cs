using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "tls-inspection-policies", "list")]
public record GcloudNetworkSecurityTlsInspectionPoliciesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;