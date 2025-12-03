using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "authorization-policies", "list")]
public record GcloudNetworkSecurityAuthorizationPoliciesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;