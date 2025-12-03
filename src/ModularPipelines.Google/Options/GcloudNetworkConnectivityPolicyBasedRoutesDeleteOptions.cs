using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "policy-based-routes", "delete")]
public record GcloudNetworkConnectivityPolicyBasedRoutesDeleteOptions(
[property: CliArgument] string PolicyBasedRoute
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}