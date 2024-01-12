using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "authorization-policies", "list")]
public record GcloudNetworkSecurityAuthorizationPoliciesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;