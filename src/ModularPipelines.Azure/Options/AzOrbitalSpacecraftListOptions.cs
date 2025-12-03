using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("orbital", "spacecraft", "list")]
public record AzOrbitalSpacecraftListOptions : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--skiptoken")]
    public string? Skiptoken { get; set; }
}