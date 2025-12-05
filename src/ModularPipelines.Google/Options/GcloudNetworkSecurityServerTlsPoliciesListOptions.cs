using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "server-tls-policies", "list")]
public record GcloudNetworkSecurityServerTlsPoliciesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;