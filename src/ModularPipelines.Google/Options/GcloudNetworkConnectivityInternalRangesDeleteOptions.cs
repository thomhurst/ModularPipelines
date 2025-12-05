using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "internal-ranges", "delete")]
public record GcloudNetworkConnectivityInternalRangesDeleteOptions(
[property: CliArgument] string InternalRange,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}