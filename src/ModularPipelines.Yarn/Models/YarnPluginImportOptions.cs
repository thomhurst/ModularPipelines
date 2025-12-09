using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("plugin", "import")]
public record YarnPluginImportOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Name
) : YarnOptions
{
    [CliFlag("--checksum")]
    public virtual bool? Checksum { get; set; }
}