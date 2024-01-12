using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "policy-based-routes", "delete")]
public record GcloudNetworkConnectivityPolicyBasedRoutesDeleteOptions(
[property: PositionalArgument] string PolicyBasedRoute
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}