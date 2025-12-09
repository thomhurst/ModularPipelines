using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "packet-capture", "create")]
public record AzNetworkWatcherPacketCaptureCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--capture-limit")]
    public string? CaptureLimit { get; set; }

    [CliOption("--capture-size")]
    public string? CaptureSize { get; set; }

    [CliOption("--exclude")]
    public string? Exclude { get; set; }

    [CliOption("--file-path")]
    public string? FilePath { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--include")]
    public string? Include { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--storage-path")]
    public string? StoragePath { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--target-type")]
    public string? TargetType { get; set; }

    [CliOption("--time-limit")]
    public string? TimeLimit { get; set; }

    [CliOption("--vm")]
    public string? Vm { get; set; }
}