using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "watcher", "configure")]
public record AzNetworkWatcherConfigureOptions(
[property: CommandSwitch("--locations")] string Locations
) : AzOptions
{
    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}