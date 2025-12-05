using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("npm", "tag", "add")]
public record YarnNpmTagAddOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Package,
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Tag
) : YarnOptions;