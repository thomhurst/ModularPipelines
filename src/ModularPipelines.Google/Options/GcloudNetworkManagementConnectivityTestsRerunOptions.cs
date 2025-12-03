using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-management", "connectivity-tests", "rerun")]
public record GcloudNetworkManagementConnectivityTestsRerunOptions(
[property: CliArgument] string ConnectivityTest
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}