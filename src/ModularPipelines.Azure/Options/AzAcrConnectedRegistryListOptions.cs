using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "connected-registry", "list")]
public record AzAcrConnectedRegistryListOptions(
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliFlag("--no-children")]
    public bool? NoChildren { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}