using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "operation-abort", "(aks-preview", "extension)")]
public record AzAksOperationAbortAksPreviewExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}