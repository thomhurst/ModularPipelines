using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("plugin", "remove")]
public record YarnPluginRemoveOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Name
) : YarnOptions;