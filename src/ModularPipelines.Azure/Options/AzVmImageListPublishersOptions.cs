using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "image", "list-publishers")]
public record AzVmImageListPublishersOptions(
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }
}