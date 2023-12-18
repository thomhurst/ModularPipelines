using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "browse")]
public record AzAksBrowseOptions(
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