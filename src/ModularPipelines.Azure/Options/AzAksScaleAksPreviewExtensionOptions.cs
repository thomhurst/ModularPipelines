using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "scale", "(aks-preview", "extension)")]
public record AzAksScaleAksPreviewExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--node-count")] int NodeCount,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--nodepool-name")]
    public string? NodepoolName { get; set; }
}