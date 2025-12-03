using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "list-locations")]
public record AzAccountListLocationsOptions : AzOptions
{
    [CliFlag("--include-extended-locations")]
    public bool? IncludeExtendedLocations { get; set; }
}