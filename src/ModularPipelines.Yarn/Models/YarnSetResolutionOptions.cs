using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("set", "resolution")]
public record YarnSetResolutionOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Descriptor,
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Resolution
) : YarnOptions
{
    [CliFlag("--save")]
    public virtual bool? Save { get; set; }
}