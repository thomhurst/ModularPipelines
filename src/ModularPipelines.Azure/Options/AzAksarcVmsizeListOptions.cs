using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aksarc", "vmsize", "list")]
public record AzAksarcVmsizeListOptions(
[property: CommandSwitch("--custom-location")] string CustomLocation
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}