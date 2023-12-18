using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "check-health")]
public record AzAcrCheckHealthOptions : AzOptions
{
    [BooleanCommandSwitch("--ignore-errors")]
    public bool? IgnoreErrors { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}