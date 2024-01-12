using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "server-tls-policies", "list")]
public record GcloudNetworkSecurityServerTlsPoliciesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;