using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "client-tls-policies", "list")]
public record GcloudNetworkSecurityClientTlsPoliciesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;