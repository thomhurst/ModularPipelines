using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "client-tls-policies", "list")]
public record GcloudNetworkSecurityClientTlsPoliciesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;