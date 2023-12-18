using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "browse", "(aks-preview", "extension)")]
public record AzAksBrowseAksPreviewExtensionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--disable-browser")]
    public bool? DisableBrowser { get; set; }

    [CommandSwitch("--listen-address")]
    public string? ListenAddress { get; set; }

    [CommandSwitch("--listen-port")]
    public string? ListenPort { get; set; }
}

