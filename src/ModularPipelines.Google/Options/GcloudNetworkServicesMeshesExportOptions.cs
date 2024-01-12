using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "meshes", "export")]
public record GcloudNetworkServicesMeshesExportOptions(
[property: PositionalArgument] string Mesh,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}