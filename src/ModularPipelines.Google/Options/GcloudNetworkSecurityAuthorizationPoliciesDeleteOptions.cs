using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "authorization-policies", "delete")]
public record GcloudNetworkSecurityAuthorizationPoliciesDeleteOptions(
[property: CliArgument] string AuthorizationPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}