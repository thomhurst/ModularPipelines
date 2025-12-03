using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aksarc", "vmsize", "list")]
public record AzAksarcVmsizeListOptions(
[property: CliOption("--custom-location")] string CustomLocation
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}