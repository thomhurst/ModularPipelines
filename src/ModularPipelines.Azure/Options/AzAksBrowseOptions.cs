using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "browse")]
public record AzAksBrowseOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--disable-browser")]
    public bool? DisableBrowser { get; set; }

    [CliOption("--listen-address")]
    public string? ListenAddress { get; set; }

    [CliOption("--listen-port")]
    public string? ListenPort { get; set; }
}