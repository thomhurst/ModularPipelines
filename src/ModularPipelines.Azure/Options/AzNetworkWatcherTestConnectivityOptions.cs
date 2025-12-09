using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "test-connectivity")]
public record AzNetworkWatcherTestConnectivityOptions(
[property: CliOption("--source-resource")] string SourceResource
) : AzOptions
{
    [CliOption("--dest-address")]
    public string? DestAddress { get; set; }

    [CliOption("--dest-port")]
    public string? DestPort { get; set; }

    [CliOption("--dest-resource")]
    public string? DestResource { get; set; }

    [CliOption("--headers")]
    public string? Headers { get; set; }

    [CliOption("--method")]
    public string? Method { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--source-port")]
    public string? SourcePort { get; set; }

    [CliOption("--valid-status-codes")]
    public string? ValidStatusCodes { get; set; }
}