using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "delete")]
public record GcloudNetworkConnectivityHubsDeleteOptions(
[property: CliArgument] string Hub
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}