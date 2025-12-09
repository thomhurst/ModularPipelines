using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("exec")]
public record YarnExecOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string CommandName
) : YarnOptions;