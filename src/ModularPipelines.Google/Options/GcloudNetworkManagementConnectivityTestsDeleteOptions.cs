using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-management", "connectivity-tests", "delete")]
public record GcloudNetworkManagementConnectivityTestsDeleteOptions(
[property: CliArgument] string ConnectivityTest
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}