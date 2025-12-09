using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("workspace")]
public record YarnWorkspaceOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string WorkspaceName,
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string CommandName
) : YarnOptions;