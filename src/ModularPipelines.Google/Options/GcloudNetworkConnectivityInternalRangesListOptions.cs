using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "internal-ranges", "list")]
public record GcloudNetworkConnectivityInternalRangesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}