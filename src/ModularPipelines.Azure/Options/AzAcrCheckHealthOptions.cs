using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "check-health")]
public record AzAcrCheckHealthOptions : AzOptions
{
    [CliFlag("--ignore-errors")]
    public bool? IgnoreErrors { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}