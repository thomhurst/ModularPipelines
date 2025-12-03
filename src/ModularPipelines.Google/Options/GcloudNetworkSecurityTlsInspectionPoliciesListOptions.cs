using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "tls-inspection-policies", "list")]
public record GcloudNetworkSecurityTlsInspectionPoliciesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;