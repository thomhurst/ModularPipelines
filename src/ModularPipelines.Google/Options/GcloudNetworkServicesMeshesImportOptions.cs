using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "meshes", "import")]
public record GcloudNetworkServicesMeshesImportOptions(
[property: CliArgument] string Mesh,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}