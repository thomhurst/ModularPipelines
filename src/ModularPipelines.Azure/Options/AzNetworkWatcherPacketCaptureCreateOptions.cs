using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "packet-capture", "create")]
public record AzNetworkWatcherPacketCaptureCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--capture-limit")]
    public string? CaptureLimit { get; set; }

    [CommandSwitch("--capture-size")]
    public string? CaptureSize { get; set; }

    [CommandSwitch("--exclude")]
    public string? Exclude { get; set; }

    [CommandSwitch("--file-path")]
    public string? FilePath { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--include")]
    public string? Include { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--storage-path")]
    public string? StoragePath { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--target-type")]
    public string? TargetType { get; set; }

    [CommandSwitch("--time-limit")]
    public string? TimeLimit { get; set; }

    [CommandSwitch("--vm")]
    public string? Vm { get; set; }
}

