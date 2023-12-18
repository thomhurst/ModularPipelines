using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "connected-registry", "list")]
public record AzAcrConnectedRegistryListOptions(
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [BooleanCommandSwitch("--no-children")]
    public bool? NoChildren { get; set; }

    [CommandSwitch("--parent")]
    public string? Parent { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}