using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "list-locations")]
public record AzAccountListLocationsOptions : AzOptions
{
    [BooleanCommandSwitch("--include-extended-locations")]
    public bool? IncludeExtendedLocations { get; set; }
}