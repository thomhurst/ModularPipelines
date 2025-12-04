using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "watcher", "configure")]
public record AzNetworkWatcherConfigureOptions(
[property: CliOption("--locations")] string Locations
) : AzOptions
{
    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}