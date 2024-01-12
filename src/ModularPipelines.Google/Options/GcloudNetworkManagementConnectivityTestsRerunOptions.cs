using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-management", "connectivity-tests", "rerun")]
public record GcloudNetworkManagementConnectivityTestsRerunOptions(
[property: PositionalArgument] string ConnectivityTest
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}