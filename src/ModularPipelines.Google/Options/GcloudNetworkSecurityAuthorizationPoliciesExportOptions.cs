using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "authorization-policies", "export")]
public record GcloudNetworkSecurityAuthorizationPoliciesExportOptions(
[property: PositionalArgument] string AuthorizationPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}