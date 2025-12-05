using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "network", "private-endpoint", "connection", "wait")]
public record AzDtNetworkPrivateEndpointConnectionWaitOptions(
[property: CliOption("--cn")] string Cn,
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliOption("--custom")]
    public string? Custom { get; set; }

    [CliFlag("--deleted")]
    public bool? Deleted { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--updated")]
    public bool? Updated { get; set; }
}