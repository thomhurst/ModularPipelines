using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "test-connectivity")]
public record AzNetworkWatcherTestConnectivityOptions(
[property: CommandSwitch("--source-resource")] string SourceResource
) : AzOptions
{
    [CommandSwitch("--dest-address")]
    public string? DestAddress { get; set; }

    [CommandSwitch("--dest-port")]
    public string? DestPort { get; set; }

    [CommandSwitch("--dest-resource")]
    public string? DestResource { get; set; }

    [CommandSwitch("--headers")]
    public string? Headers { get; set; }

    [CommandSwitch("--method")]
    public string? Method { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--source-port")]
    public string? SourcePort { get; set; }

    [CommandSwitch("--valid-status-codes")]
    public string? ValidStatusCodes { get; set; }
}