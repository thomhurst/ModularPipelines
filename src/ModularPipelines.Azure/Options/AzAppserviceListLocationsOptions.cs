using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "list-locations")]
public record AzAppserviceListLocationsOptions(
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [BooleanCommandSwitch("--hyperv-workers-enabled")]
    public bool? HypervWorkersEnabled { get; set; }

    [BooleanCommandSwitch("--linux-workers-enabled")]
    public bool? LinuxWorkersEnabled { get; set; }
}