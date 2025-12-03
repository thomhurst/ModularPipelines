using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "service-connection-policies", "delete")]
public record GcloudNetworkConnectivityServiceConnectionPoliciesDeleteOptions(
[property: CliArgument] string ServiceConnectionPolicy,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}