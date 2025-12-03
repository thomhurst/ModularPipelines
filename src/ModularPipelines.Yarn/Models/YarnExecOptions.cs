using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("exec")]
public record YarnExecOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string CommandName
) : YarnOptions;