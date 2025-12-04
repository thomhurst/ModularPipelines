using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appservice", "list-locations")]
public record AzAppserviceListLocationsOptions(
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliFlag("--hyperv-workers-enabled")]
    public bool? HypervWorkersEnabled { get; set; }

    [CliFlag("--linux-workers-enabled")]
    public bool? LinuxWorkersEnabled { get; set; }
}